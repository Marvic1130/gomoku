package com.gamza.gomoku.error.execption;

import com.gamza.gomoku.error.ErrorCode;

public class DuplicateException extends BusinessException {

  public DuplicateException(String message, ErrorCode errorCode) {
    super(message, errorCode);
  }
}
