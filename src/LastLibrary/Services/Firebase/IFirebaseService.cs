using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireSharp.Interfaces;

namespace LastLibrary.Services.Firebase
{
    public interface IFirebaseService
    {
        Task WriteToFirebase();
    }
}
