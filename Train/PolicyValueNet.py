import torch
import torch.nn as nn


def softmax(x):
    # probs = torch.exp(x - torch.max(x))
    x = x - torch.max(x)
    probs = torch.exp(x)
    return probs / torch.sum(probs)

def relu(X):
    out = torch.maximum(X, torch.tensor(0))
    return out

def conv_forward(X, W, b, stride=1, padding=1):
    n_filters, d_filter, h_filter, w_filter = W.shape
    W = W[:, :, :, ::-1, ::-1]
    n_x, d_x, h_x, w_x = X.shape
    h_out = (h_x - h_filter + 2 * padding) / stride + 1
    w_out = (w_x - w_filter + 2 * padding) / stride + 1
    h_out, w_out = int(h_out), int(w_out)
    X_col = im2col_indices(X, h_filter, w_filter, padding=padding, stride=stride)
    W_col = W.reshape(n_filters, -1)
    out = (torch.mm(W_col, X_col.t()) + b).t()
    out = out.reshape(n_x, n_filters, h_out, w_out)
    out = out.permute(0, 1, 3, 2)
    return out

def fc_forward(X, W, b):
    out = torch.mm(X, W) + b
    return out

def get_im2col_indices(x_shape, field_height, field_width, padding=1, stride=1):
    N, C, H, W = x_shape
    assert (H + 2 * padding - field_height) % stride == 0
    assert (W + 2 * padding - field_height) % stride == 0
    out_height = int((H + 2 * padding - field_height) / stride + 1)
    out_width = int((W + 2 * padding - field_width) / stride + 1)

    i0 = torch.repeat_interleave(torch.arange(field_height), field_width).repeat(C)
    i1 = stride * torch.repeat_interleave(torch.arange(out_height), out_width)
    j0 = torch.tile(torch.arange(field_width), field_height * C)
    j1 = stride * torch.tile(torch.arange(out_width), (out_height,))
    i = i0.reshape(-1, 1) + i1.reshape(1, -1)
    j = j0.reshape(-1, 1) + j1.reshape(1, -1)

    k = torch.repeat_interleave(torch.arange(C), field_height * field_width).reshape(-1, 1)

    return (k.int(), i.int(), j.int())

def im2col_indices(x, field_height, field_width, padding=1, stride=1):
    p = padding
    x_padded = torch.nn.functional.pad(x, (p, p, p, p))

    k, i, j = get_im2col_indices(x.shape, field_height, field_width, padding, stride)

    cols = x_padded[:, k, i, j]
    C = x.shape[1]
    cols = cols.permute(1, 2, 0).reshape(field_height * field_width * C, -1)
    return cols

class res_block(nn.Module):
    def __init__(self, in_channels, out_channels, kernel_size=3, stride=1, padding=1):
        super(res_block, self).__init__()
        self.conv1 = nn.Conv2d(in_channels, out_channels, kernel_size, stride, padding)
        self.conv2 = nn.Conv2d(out_channels, out_channels, kernel_size, stride, padding)
        self.bn1 = nn.BatchNorm2d(out_channels)
        self.bn2 = nn.BatchNorm2d(out_channels)
        self.relu1 = nn.LeakyReLU()
        self.relu2 = nn.LeakyReLU()

    def forward(self, x):
        out = self.conv1(x)
        out = self.bn1(out)
        out = self.relu1(out)
        out = self.conv2(out)
        out = self.bn2(out)
        out += x
        out = self.relu2(out)
        return out

class PolicyResNet(nn.Module):
    def __init__(self, board_width, board_height, model_file=None):
        super(PolicyResNet, self).__init__()
        # self.board_width = board_width
        # self.board_height = board_height
        # self.learning_rate = LEARNING_RATE
        # self.l2_const = L2_CONST  # coef of l2 penalty
        self.device = "cuda" if torch.cuda.is_available() else "mps" if torch.backends.mps.is_available() else "cpu"

        self.conv1 = nn.Conv2d(4, 32, kernel_size=3, padding=1)
        self.bn1 = nn.BatchNorm2d(32)
        self.relu1 = nn.ReLU()
        self.conv2 = nn.Conv2d(32, 64, kernel_size=3, padding=1)
        self.bn2 = nn.BatchNorm2d(64)
        self.relu2 = nn.ReLU()
        self.block1 = res_block(64, 64)
        self.block2 = res_block(64, 64)
        # self.block3 = res_block(64, 64)

        self.value_conv = nn.Conv2d(64, 32, kernel_size=1)
        self.value_bn = nn.BatchNorm2d(32)
        self.value_relu1 = nn.LeakyReLU()
        self.value_fc1 = nn.Linear(board_width * board_height*32, 64)
        self.value_relu2 = nn.LeakyReLU()
        self.value_fc2 = nn.Linear(64, 1)

        self.policy_conv = nn.Conv2d(64, 16, kernel_size=1)
        self.policy_bn = nn.BatchNorm2d(16)
        self.policy_relu1 = nn.LeakyReLU()
        self.policy_fc1 = nn.Linear(board_width * board_height * 16, board_width * board_height)

        if model_file:
            net_params = torch.load(model_file)
            self.load_state_dict(net_params)

        self.to(torch.device(self.device))
    def forward(self, state_input):
        x = self.conv1(state_input)
        x = self.bn1(x)
        x = self.relu1(x)
        x = self.conv2(x)
        x = self.bn2(x)
        x = self.relu2(x)
        x = self.block1(x)
        x = self.block2(x)
        # x = self.block3(x)

        value = self.value_conv(x)
        value = self.value_bn(value)
        value = self.value_relu1(value)
        value = value.flatten(start_dim=1)
        value = self.value_fc1(value)
        value = self.value_relu2(value)
        value = self.value_fc2(value)
        value = torch.tanh(value)

        policy = self.policy_conv(x)
        policy = self.policy_bn(policy)
        policy = self.policy_relu1(policy)
        policy = policy.flatten(start_dim=1)
        policy = self.policy_fc1(policy)

        return softmax(policy), value[0]

    def policy_value_fn(self, board):
        legal_positions = list(set(range(board.width * board.height)) - set(board.states.keys()))
        current_state = board.current_state()
        current_state_tensor = torch.tensor(current_state, dtype=torch.float32).unsqueeze(0).to(self.device)
        policy, value = self(current_state_tensor)  # forward 호출
        policy = policy.cpu()
        policy = zip(legal_positions, policy.flatten()[legal_positions].detach().numpy())
        value = value.cpu()
        return policy, value.detach().numpy()

    def save_model(self, model_file):
        torch.save(self.state_dict(), model_file)

    def load_model(self, model_path):
        self.load_state_dict(torch.load(model_path))


class PolicyValueNet(nn.Module):
    def __init__(self, board_width, board_height, model_file=None):
        super(PolicyValueNet, self).__init__()
        self.device = torch.device("cuda" if torch.cuda.is_available() else
                                   "mps" if torch.backends.mps.is_available() else
                                   "cpu")
        # self.board_width = board_width
        # self.board_height = board_height
        # self.learning_rate = LEARNING_RATE
        # self.l2_const = L2_CONST  # coef of l2 penalty

        self.conv1 = nn.Conv2d(4, 32, kernel_size=3, padding=1)
        self.conv2 = nn.Conv2d(32, 64, kernel_size=3, padding=1)
        self.conv3 = nn.Conv2d(64, 128, kernel_size=3, padding=1)
        self.policy_conv = nn.Conv2d(128, 4, kernel_size=1)
        self.policy_fc1 = nn.Linear(board_width * board_height, board_width * board_height)
        self.value_conv = nn.Conv2d(128, 2, kernel_size=1)
        self.value_fc1 = nn.Linear(board_width*board_height*2, 64)
        self.value_fc2 = nn.Linear(64, 1)

        if model_file:
            net_params = torch.load(model_file)
            self.load_state_dict(net_params)

        self.to(self.device)

    def forward(self, state_input):
        x = state_input
        x = relu(self.conv1(x))
        x = relu(self.conv2(x))
        x = relu(self.conv3(x))

        policy = relu(self.policy_conv(x))
        policy = policy.flatten(start_dim=1)

        value = relu(self.value_conv(x))
        value = relu(self.value_fc1(value.flatten(start_dim=1)))
        value = torch.tanh(self.value_fc2(value))

        return softmax(policy), value[0]

    def policy_value_fn(self, board):
        legal_positions = list(set(range(board.width * board.height)) - set(board.states.keys()))
        current_state = board.current_state()
        current_state_tensor = torch.tensor(current_state, dtype=torch.float32).unsqueeze(0).to(self.device)
        policy, value = self(current_state_tensor)  # forward 호출
        policy = policy.cpu()
        policy = zip(legal_positions, policy.flatten()[legal_positions].detach().numpy())
        value = value.cpu()
        return policy, value.detach().numpy()

    def save_model(self, model_file):
        torch.save(self.state_dict(), model_file)

    def load_model(self, model_path):
        self.load_state_dict(torch.load(model_path))
