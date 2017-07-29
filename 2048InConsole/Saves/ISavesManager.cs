using System;
using System.Collections.Generic;

namespace _2048InConsole.Saves
{
    interface ISavesManager<T>
    {
        bool TrySave(string filePath, T item, IEnumerable<Type> knownTypes);

        bool TryLoad(string filePath, IEnumerable<Type> knownTypes, out T item);
    }
}
