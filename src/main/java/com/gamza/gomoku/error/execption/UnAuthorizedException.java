package com.gamza.gomoku.error.execption;

import com.gamza.gomoku.error.ErrorCode;
import lombok.Getter;


@Getter
public class UnAuthorizedException extends BusinessException {

  public UnAuthorizedException(String message, ErrorCode errorCode) {
    super(message,errorCode);
  }
}
