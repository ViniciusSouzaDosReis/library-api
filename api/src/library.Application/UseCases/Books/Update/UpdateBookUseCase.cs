using AutoMapper;
using library.Communication.Requests;
using library.Domain.Repositories;
using library.Domain.Repositories.Books;
using library.Exception;
using library.Exception.Book;
using library.Exception.ExceptionBase;
using System.ComponentModel.DataAnnotations;

namespace library.Application.UseCases.Books.Update;

public class UpdateBookUseCase : IUpdateBookUseCase
{
    private readonly IBookUpdateOnlyRepositores _bookRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookUseCase(IBookUpdateOnlyRepositores bookRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id, RequestBookJson request)
    {
        Validate(request);

        var book = await _bookRepository.GetById(id);

        if(book is null)
        {
            throw new NotFoundException(ResourceErrorMessage.BOOK_NOT_FOUND);
        }

        _bookRepository.Update(book);

        _mapper.Map(request, book);
        await _unitOfWork.Commit();
    }

    private void Validate(RequestBookJson book)
    {
        var validate = new BookValidate();

        var result = validate.Validate(book);

        if (result.IsValid == false)
        {
            var errorMessage = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessage);
        }
    }
}
