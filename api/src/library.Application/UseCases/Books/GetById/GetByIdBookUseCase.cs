using AutoMapper;
using library.Communication.Responses;
using library.Communication.Responses.Book;
using library.Domain.Repositories.Books;
using library.Exception.Book;
using library.Exception.ExceptionBase;
using System.Net;

namespace library.Application.UseCases.Books.GetById;

public class GetByIdBookUseCase : IGetByIdBookUseCase
{
    private readonly IBookReadOnlyRepositories _bookRepository;
    private readonly IMapper _mapper;

    public GetByIdBookUseCase(IBookReadOnlyRepositories bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ResponseBookJson>> Execute(Guid id)
    {
        var book = await _bookRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessage.BOOK_NOT_FOUND);

        var response = ApiResponse.CreateSuccesResponseWithData(
            _mapper.Map<ResponseBookJson>(book),
            (int)HttpStatusCode.OK);

        return response;
    }
}
