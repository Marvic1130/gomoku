package com.gamza.gomoku.error.execption;


import com.gamza.gomoku.error.ErrorCode;
import lombok.Getter;

@Getter
public class NotFoundException extends BusinessException{

    public NotFoundException(String message, ErrorCode errorCode) {
        super(message, errorCode);
    }
}
