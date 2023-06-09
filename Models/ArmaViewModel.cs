using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgMvc.Models
{
    public class ArmaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public int Dano { get; set; }

        public PersonagemViewModel Personagem { get; set; }
        public int PersonagemId { get; set; }
        public int ArmaId { get; set; }

        public ArmaViewModel Arma {get; set;}
    }
}