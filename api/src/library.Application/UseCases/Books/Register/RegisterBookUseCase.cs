using AutoMapper;
using library.Application.Helpers;
using library.Communication.Requests;
using library.Communication.Responses;
using library.Communication.Responses.Book;
using library.Domain.Entities;
using library.Domain.Enums;
using library.Domain.Repositories;
using library.Domain.Repositories.Books;
using library.Exception.ExceptionBase;
using System.Net;

namespace library.Application.UseCases.Books.Register;

public class RegisterBookUseCase : IRegisterBookUseCase
{
    private readonly IBookWriteOnlyRepositories _bookRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uniOfWork;

    public RegisterBookUseCase(
        IBookWriteOnlyRepositories bookRepository,
        IMapper mapper,
        IUnitOfWork uniOfWork)
    {
        _mapper = mapper;
        _bookRepository = bookRepository;
        _uniOfWork = uniOfWork;
    }

    public async Task<ApiResponse> Execute(RequestBookJson request)
    {
        Validate(request);

        var book = ConvertRequestToEntity(request);

        await _bookRepository.Add(book);

        await _uniOfWork.Commit();

        return ApiResponse.CreateSuccesResponse((int)HttpStatusCode.Created);
    }

    private void Validate(RequestBookJson request)
    {
        var validate = new BookValidate();

        var result = validate.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessage = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessage);
        }
    }

    private Book ConvertRequestToEntity(RequestBookJson request)
    {
        var book = _mapper.Map<Book>(request);
        book.Id = Guid.NewGuid();

        book.Slug = GuidHelper.ToShortString(book.Id);

        return book;
    }
}
