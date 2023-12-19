package com.gamza.gomoku.error.execption;


import com.gamza.gomoku.error.ErrorCode;

public class BadRequestException extends BusinessException{

    public BadRequestException(String message, ErrorCode errorCode) {
        super(message,errorCode);
    }
}
