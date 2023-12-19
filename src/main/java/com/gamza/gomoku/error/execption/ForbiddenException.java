package com.gamza.gomoku.error.execption;

import com.gamza.gomoku.error.ErrorCode;

public class ForbiddenException extends BusinessException {

    public ForbiddenException(String message, ErrorCode errorCode) {
        super(message, errorCode);
    }
}
