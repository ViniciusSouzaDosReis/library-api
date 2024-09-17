using AutoMapper;
using library.Communication.Responses;
using library.Communication.Responses.Book;
using library.Domain.Repositories.Books;
using System.Net;

namespace library.Application.UseCases.Books.GetAll;

public class GetAllBooksUseCase : IGetAllBooksUseCase
{
    private readonly IBookReadOnlyRepositories _bookRepository;
    private readonly IMapper _mapper;

    public GetAllBooksUseCase(IBookReadOnlyRepositories bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ICollection<ResponseBookJson>>> Execute()
    {
        var books = await _bookRepository.GetAll();

        var response = ApiResponse.CreateSuccesResponseWithData(
            _mapper.Map<ICollection<ResponseBookJson>>(books),
            (int)HttpStatusCode.OK);

        return response;
    }
}
