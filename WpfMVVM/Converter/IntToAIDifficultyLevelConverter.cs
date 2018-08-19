using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMVVM.Model;

namespace WpfMVVM.Converter
{
    class IntToAIDifficultyLevelConverter
    {
        private readonly Dictionary<int, AIDifficultyLevel> _mapper;

        public IntToAIDifficultyLevelConverter()
        {
            _mapper = new Dictionary<int, AIDifficultyLevel>
            {
                {0, AIDifficultyLevel.Easy},
                {1, AIDifficultyLevel.Medium},
                {2, AIDifficultyLevel.Hard}
            };
        }

        public AIDifficultyLevel Convert(int value)
        {
            return _mapper[value];
        }
    }
}
