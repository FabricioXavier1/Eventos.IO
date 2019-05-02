using Eventos.IO.Domain.Interfaces;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventos.IO.Domain.CommandHandlers
{
    public abstract class CommandHandler
    {
        private readonly IUnitOfWork _uow;

        public CommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        protected void NotificarValidacoesErro(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }

        protected bool Commit()
        {
            //TODO: Validar se há alguma validação de negócio com erro.

            var commandResponse = _uow.Commit();

            if (commandResponse.Sucess) return true;

            Console.WriteLine("Ocorreu um erro ao salvar os dados");
            return false;
        }
    }
}
