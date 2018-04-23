using Partiality.Modloader;
using MonoMod.ModInterop;

namespace AbraxisToolset.src.Patches
{
    class UtilSetup : PartialityMod
    {
        public override void Init()
        {
            typeof(Utils).ModInterop();
            base.Init();
        }

    }
}
