package com.gamza.gomoku.error.execption;


import com.gamza.gomoku.error.ErrorCode;
import lombok.Getter;

@Getter
public class JwtException extends BusinessException{
    public JwtException(String message, ErrorCode errorCode){
        super(message,errorCode);
    }
}
