using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miner.Services
{
    public class Recharger
    {
        public List<Models.CardCharge> ChargeCards { get; set; }
        public Recharger()
        {
            ChargeCards = new List<Models.CardCharge>();
        }

        public List<Models.CardCharge> CreateChargeCards(int count, double value)
        {
            var Random = new Random();
            var ListNewCard = new List<Models.CardCharge>();
            for (int index = 0; index < count; index++)
            {
                var numberCard = Random.Next(100_000_000, 999_999_999);
                ListNewCard.Add(new Models.CardCharge
                {
                    Number = numberCard,
                    Value = value
                });
            }
            ChargeCards.AddRange(ListNewCard);
            return ListNewCard;
        }
        public bool UseCard(int numberCard)
        {
            var card = ChargeCards.SingleOrDefault(card => card.Number == numberCard);
            if (card == null) return false;
            return ChargeCards.Remove(card);
        }
    }

}
