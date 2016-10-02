using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FireSharp.Interfaces;
using LastLibrary.Models.DeckManagerViewModel;

namespace LastLibrary.Services.Firebase
{
    public interface IFirebaseService
    {
        Task<HttpStatusCode> WriteToFirebase(DeckModel deckModel);
    }
}