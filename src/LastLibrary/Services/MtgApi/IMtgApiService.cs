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
        CardsModel SearchForCards(string cardName);
        CardsModel SearchForCards(string cardName, CardSearchOptionsModel cardOpts);
    }
}
