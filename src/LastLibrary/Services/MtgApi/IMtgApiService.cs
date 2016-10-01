using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using LastLibrary.Models;

namespace LastLibrary.Services.MtgApi
{
    public interface IMtgApiService
    {
        Task<CardsModel> SearchForCards(string cardName);
        Task<CardsModel> SearchForCards(string cardName, dynamic cardOpts);
    }
}
