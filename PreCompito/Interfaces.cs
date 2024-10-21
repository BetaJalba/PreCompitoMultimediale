using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreCompito
{
    public interface IVolumeControls 
    {
        void Weaker(int amount);
        void Louder(int amount);
    }

    public interface IBrightnessControls 
    {
        void Brighter(int amount);
        void Darker(int amount);
    }
}
