﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class Customer
    {
        //Member Variables (Has A)
        public Wallet Wallet;
        public Backpack Backpack;

        //Constructor (Spawner)
        public Customer()
        {
            Wallet = new Wallet();
            Backpack = new Backpack();
        }
        //Member Methods (Can Do)

        //This method will be the main logic for a customer to retrieve coins form their wallet.
        //Takes in the selected can for price reference
        //Will need to get user input for coins they would like to add.
        //When all is said and done this method will return a list of coin objects that the customer will use a payment for their soda.
        public List<Coin> GatherCoinsFromWallet(Can selectedCan)
        {
            double canPrice = selectedCan.Price;
            List<Coin> selectedCoins = new List<Coin>();
            Console.WriteLine($"The can costs {canPrice}.");
            while(canPrice > 0)
            {
                string coinName = UserInterface.CoinSelection(selectedCan, selectedCoins);
                Coin coin = GetCoinFromWallet(coinName);
                if(coin != null)
                {
                    selectedCoins.Add(coin);
                    canPrice -= coin.Value;
                }
            }
            return selectedCoins;
        }
        //Returns a coin object from the wallet based on the name passed into it.
        //Returns null if no coin can be found
        public Coin GetCoinFromWallet(string coinName)
        {
            foreach (Coin coin in Wallet.Coins)
            {
                if (coin.Name == coinName)
                {
                    Wallet.Coins.Remove(coin);
                    return coin;
                }
            }
            return null;
        }
        //Takes in a list of coin objects to add into the customers wallet.
        public void AddCoinsIntoWallet(List<Coin> coinsToAdd)
        {
            //look at AddRange for list as well
            foreach (Coin coin in coinsToAdd)
            {
                Wallet.Coins.Add(coin);
            }
        }
        //Takes in a can object to add to the customers backpack.
        public void AddCanToBackpack(Can purchasedCan)
        {
            Backpack.cans.Add(purchasedCan);
            Console.WriteLine($"The {purchasedCan} has been added to your backpack");
        }
    }
}
