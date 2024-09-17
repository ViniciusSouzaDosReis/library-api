﻿using AutoMapper;
using library.Communication.Responses;
using library.Communication.Responses.Book;
using library.Domain.Repositories.Books;
using library.Exception.Book;
using library.Exception.ExceptionBase;
using System.Net;

namespace library.Application.UseCases.Books.GetBySlug;

public class GetBySlugBookUseCase : IGetBySlugBookUseCase
{
    private readonly IBookReadOnlyRepositories _bookRepository;
    private readonly IMapper _mapper;

    public GetBySlugBookUseCase(IBookReadOnlyRepositories bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ResponseBookGetBySlugJson>> Execute(string slug)
    {
        var book = await _bookRepository.GetBySlug(slug) ?? throw new NotFoundException(ResourceErrorMessage.BOOK_NOT_FOUND);

        var response = ApiResponse.CreateSuccesResponseWithData(
            _mapper.Map<ResponseBookGetBySlugJson>(book),
            (int)HttpStatusCode.OK);
        return response;
    }
}