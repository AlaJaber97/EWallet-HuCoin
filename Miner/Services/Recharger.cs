using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miner.Services
{
    public class Recharger
    {
        private List<Models.CardCharge> ChargeCards { get; set; }
        public Recharger()
        {
            ChargeCards = new List<Models.CardCharge>();
        }
        public List<Models.CardCharge> GetChargeCards() => ChargeCards;
        public List<Models.CardCharge> CreateChargeCards(int count, decimal value)
        {
            var Random = new Random();
            var ListNewCard = new List<Models.CardCharge>();
            for (int index = 0; index < count; index++)
            {
                int number_card;
                //do
                //{
                    number_card = Random.Next(100_000_000, 999_999_999);
                //} while (ChargeCards.Find(item => item.NumberCard == number_card) != null);

                ListNewCard.Add(new Models.CardCharge
                {
                    NumberCard = number_card,
                    Value = value
                });
            }
            ChargeCards.AddRange(ListNewCard);
            return ListNewCard;
        }

        public bool UseCard(Models.CardCharge card) => ChargeCards.Remove(card);
    }
}
