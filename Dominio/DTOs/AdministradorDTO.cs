﻿using System.ComponentModel.DataAnnotations;
using minimal_api.Dominio.Enuns;

namespace minimal_api.DTOs
{
    public record AdministradorDTO
    {       
        public string Email { get; set; } = default!;
        public string Senha { get; set; } = default!;
        public Perfil? Perfil { get; set; } = default!;
    }
}
