using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Figures
{
	class Capsule : IFigure
	{
		private readonly CapsuleOptions _options;

		private readonly int layers = 10;

		public Capsule(IOption option)
		{
			if (option is CapsuleOptions opt)
				_options = opt;

			else
				throw new ArgumentException("Options is invalid");
		}

        public Mesh GetMesh()
        {
            throw new NotImplementedException();
        }
    }


}
