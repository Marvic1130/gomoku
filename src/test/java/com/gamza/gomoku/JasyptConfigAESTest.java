package com.gamza.gomoku;

import org.jasypt.encryption.pbe.StandardPBEStringEncryptor;
import org.jasypt.iv.RandomIvGenerator;
import org.junit.jupiter.api.Test;

class JasyptConfigAESTest {

    @Test
    void stringEncryptor() {
        String url = "jdbc:mysql://test.c0xvfvhlcqd9.ap-northeast-2.rds.amazonaws.com:3306/gomoku";
        String tokenkey = "moullekeyincode256abcd1234efgh56ahdmffodkaghzhem";
        String username = "admin";
        String password = "jihyeon1234";
        System.out.println(jasyptEncoding(url));
        System.out.println(jasyptEncoding(username));
        System.out.println(jasyptEncoding(password));
        System.out.println(jasyptEncoding(tokenkey));

    }

    public String jasyptEncoding(String value) {

        String key = "qlalfqjsgh";
        StandardPBEStringEncryptor pbeEnc = new StandardPBEStringEncryptor();
        pbeEnc.setAlgorithm("PBEWITHHMACSHA512ANDAES_256");
        pbeEnc.setPassword(key);
        pbeEnc.setIvGenerator(new RandomIvGenerator());
        return pbeEnc.encrypt(value);
    }
}