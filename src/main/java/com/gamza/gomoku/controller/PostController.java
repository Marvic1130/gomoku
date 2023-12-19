package com.gamza.gomoku.controller;

import com.gamza.gomoku.dto.post.PostListResponseDto;
import com.gamza.gomoku.dto.post.PostRequestDto;
import com.gamza.gomoku.dto.post.PostResponseDto;
import com.gamza.gomoku.error.ErrorCode;
import com.gamza.gomoku.error.execption.BadRequestException;
import com.gamza.gomoku.service.PostService;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;

@RestController
@RequestMapping("/post")
@RequiredArgsConstructor
@Tag(name = "게시글 API")
public class PostController {
    private final PostService postService;
    @PostMapping()
    public ResponseEntity<String> createPost(@RequestBody PostRequestDto postRequestDto, HttpServletRequest request) {
        return postService.createPost(postRequestDto,request);
    }
    @GetMapping("/{id}")
    public PostResponseDto getOnePostWithId(@PathVariable long id) {
        return postService.getOnePostWithId(id);
    }
    @GetMapping("/list")
    public Page<PostListResponseDto> getPostList(@RequestParam("page")int page) {
        return postService.getPostList(page);
    }
    @GetMapping("/search")
    public Page<PostListResponseDto> searchOfPostInTitleAndText(@RequestParam("key")String key, @RequestParam("page")int page) {
        if (key == null) {
            throw new BadRequestException(ErrorCode.BAD_REQUEST.getMessage(), ErrorCode.BAD_REQUEST);
        }
        return postService.searchOfPostInTitleAndText(key,page);
    }

}
