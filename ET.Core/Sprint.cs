using System.Collections.Generic;
using System.Linq;

namespace ET.Core
{
    public class Sprint
    {
        private readonly List<Atom> _atoms;
        private int index = 0;

        public Sprint(List<Atom> atoms)
        {
            _atoms = atoms.ToList();
            _atoms.Shuffle();
        }

        public Atom GetNext()
        {
            if (index > _atoms.Count)
                return null;

            return _atoms[index++];
        }
    }
}