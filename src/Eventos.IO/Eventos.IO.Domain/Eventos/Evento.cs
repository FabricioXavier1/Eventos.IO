﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eventos.IO.Domain.Core.Models;
using Eventos.IO.Domain.Organizadores;
using FluentValidation;

namespace Eventos.IO.Domain.Eventos
{
    public class Evento : Entity<Evento>
    {
        public Evento(string nome, DateTime dataInicio, DateTime dataFim, bool gratuito, decimal valor, bool online, string nomeEmpresa)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Gratuito = gratuito;
            Valor = valor;
            Online = online;
            NomeEmpresa = nomeEmpresa;
        }

        private Evento()
        {

        }

        #region Propriedades

        public string Nome { get; set; }
        public string DescricaoCurta { get; set; }
        public string DescricaoLonga { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Gratuito { get; set; }
        public decimal Valor { get; set; }
        public bool Online { get; set; }
        public string NomeEmpresa { get; set; }
        public Categoria Categoria { get; set; }
        public ICollection<Tags> Tags { get; set; }
        public Endereco Endereco { get; set; }
        public Organizador Organizador { get; set; }

        #endregion

        public override bool EhValido()
        {
            Validar();
            return ValidationResult.IsValid;
        }

        private void Validar()
        {
            ValidarNome();
            ValidarValor();
            ValidarData();
            ValidarLocal();
            ValidarNomeEmpresa();
            ValidationResult = Validate(this);
        }

        #region Validações

        private void ValidarNome()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome do evento precisa ser fornecido")
                .Length(2, 150).WithMessage("O nome do evento precisa ter entre 2 e 150 caracteres");
        }

        private void ValidarValor()
        {
            if (!Gratuito)
                RuleFor(c => c.Valor)
                    .ExclusiveBetween(1, 50000).WithMessage("O valor deve estar entre 1 e 50.000");
            else
                RuleFor(c => c.Valor)
                    .ExclusiveBetween(0, 0).When(e => e.Gratuito).WithMessage("O valor não deve ser diferente de 0 para um evento gratuito");
        }

        private void ValidarData()
        {
            RuleFor(c => c.DataInicio)
                .GreaterThan(c => c.DataFim).WithMessage("A data de início deve ser maior que a data de fim do evento");

            RuleFor(c => c.DataFim)
                .LessThan(DateTime.Now).WithMessage("A data de início deve ser maior que a data de fim do evento");
        }

        private void ValidarLocal()
        {
            if (Online)
                RuleFor(c => c.Endereco)
                    .Null().When(c => c.Online).WithMessage("O evento não deve possuir endereço se for um evento online");
            else
                RuleFor(c => c.Endereco)
                    .NotNull().When(c => !c.Online).WithMessage("O evento deve possuir um endereço");
        }

        private void ValidarNomeEmpresa()
        {
            RuleFor(c => c.NomeEmpresa)
                .NotEmpty().WithMessage("O nome do organizador deve ser preenchido")
                .Length(2, 150).WithMessage("O nome do organizador deve ter entre 2 e 150 caracteres");
        }
        #endregion

        public static class EventoFactory
        {
            public static Evento NovoEventoCompleto(Guid id, string nome, string descCurta, string descLonga, DateTime dataInicio, DateTime dataFim, bool gratuito, decimal valor, bool online, string nomeEmpresa, Guid? organizadorId)
            {
                var evento = new Evento()
                {
                    Id = Guid.NewGuid(),
                    Nome = nome,
                    DataInicio = dataInicio,
                    DescricaoCurta = descCurta,
                    DescricaoLonga = descLonga,
                    DataFim = dataFim,
                    Gratuito = gratuito,
                    Valor = valor,
                    Online = online,
                    NomeEmpresa = nomeEmpresa,
                };

                if (organizadorId != null)
                    evento.Organizador = new Organizador(organizadorId.Value);

                return evento;
            }
        }
    }
}
