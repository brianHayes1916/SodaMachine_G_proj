using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class SodaMachine
    {
        //Member Variables (Has A)
        private List<Coin> _register;
        private List<Can> _inventory;

        //Constructor (Spawner)
        public SodaMachine()
        {
            _register = new List<Coin>();
            _inventory = new List<Can>();
            FillInventory();
            FillRegister();
        }

        //Member Methods (Can Do)

        //A method to fill the sodamachines register with coin objects.
        public void FillRegister()
        {
            for (int i = 0; i < 20; i++)
            {
                Quarter quarter = new Quarter();
                _register.Add(quarter);
            }
            for(int i = 0; i < 10; i++)
            {
                Dime dime = new Dime();
                _register.Add(dime);
            }
            for(int i = 0; i < 20; i++)
            {
                Nickel nickel = new Nickel();
                _register.Add(nickel);
            }
            for(int i = 0; i <50; i++)
            {
                Penny penny = new Penny();
                _register.Add(penny);
            }
        }
        //A method to fill the sodamachines inventory with soda can objects.
        public void FillInventory()
        {
            for(int i = 0; i < 10; i++)
            {
                RootBeer rootBeer = new RootBeer();
                _inventory.Add(rootBeer);
            }
            for(int i = 0; i < 6; i++)
            {
                Cola cola = new Cola();
                _inventory.Add(cola);
            }
            for(int i = 0; i < 10; i++)
            {
                OrangeSoda orangeSoda = new OrangeSoda();
                _inventory.Add(orangeSoda);
            }
        }
        //Method to be called to start a transaction.
        //Takes in a customer which can be passed freely to which ever method needs it.
        public void BeginTransaction(Customer customer)
        {
            bool willProceed = UserInterface.DisplayWelcomeInstructions(_inventory);
            if (willProceed)
            {
                Transaction(customer);
            }
        }
        
        //This is the main transaction logic think of it like "runGame".  This is where the user will be prompted for the desired soda.
        //grab the desired soda from the inventory.
        //get payment from the user.
        //pass payment to the calculate transaction method to finish up the transaction based on the results.
        private void Transaction(Customer customer)
        {
           
        }
        //Gets a soda from the inventory based on the name of the soda.
        private Can GetSodaFromInventory(string nameOfSoda)
        {
                foreach (Can can in _inventory)
                {
                    if (can.Name == nameOfSoda)
                    {
                        _inventory.Remove(can);
                        return can;
                    }
                }
            return null;
        }

        //This is the main method for calculating the result of the transaction.
        //It takes in the payment from the customer, the soda object they selected, and the customer who is purchasing the soda.
        //This is the method that will determine the following:
        //If the payment is greater than the price of the soda, and if the sodamachine has enough change to return: Despense soda, and change to the customer.
        //If the payment is greater than the cost of the soda, but the machine does not have ample change: Despense payment back to the customer.
        //If the payment is exact to the cost of the soda:  Despense soda.
        //If the payment does not meet the cost of the soda: despense payment back to the customer.
        private void CalculateTransaction(List<Coin> payment, Can chosenSoda, Customer customer)
        {
            double totalPaid = TotalCoinValue(payment);
            if(totalPaid > chosenSoda.Price)
            {
                double change = DetermineChange(totalPaid, chosenSoda.Price);
                List<Coin> changeCoins = GatherChange(change);
                if(changeCoins == null)
                {
                    Console.WriteLine("Sorry there is not sufficient change to complete the transaction.");
                    customer.AddCoinsIntoWallet(payment);
                }
                else
                {
                    DepositCoinsIntoRegister(payment);
                    customer.AddCoinsIntoWallet(changeCoins);
                    customer.AddCanToBackpack(chosenSoda);
                }
            }
            else if(totalPaid == chosenSoda.Price)
            {
                DepositCoinsIntoRegister(payment);
                customer.AddCanToBackpack(chosenSoda);
            }
            else
            {
                customer.AddCoinsIntoWallet(payment);
            }
        }
        //Takes in the value of the amount of change needed.
        //Attempts to gather all the required coins from the sodamachine's register to make change.
        //Returns the list of coins as change to despense.
        //If the change cannot be made, return null.
        private List<Coin> GatherChange(double changeValue)
        {
            List<Coin> change = new List<Coin>();
            while(0.25 >= changeValue)
            {
                bool hasQuarters = RegisterHasCoin("Quarter");
                if (hasQuarters)
                {
                    Coin quarterHolder = GetCoinFromRegister("Quarter");
                    change.Add(quarterHolder);
                    changeValue -= 0.25;
                }
                else
                {
                    break;
                }
            }
            while (0.10 >= changeValue)
            {
                bool hasDimes = RegisterHasCoin("Dime");
                if (hasDimes == true)
                {
                    Coin dimeHolder = GetCoinFromRegister("Dime");
                    change.Add(dimeHolder);
                    changeValue -= 0.10;
                }
                else
                {
                    break;
                }
            }
            while (0.05 >= changeValue)
            {
                bool hasNickels = RegisterHasCoin("Nickel");
                if (hasNickels == true)
                {
                    Coin nickelHolder = GetCoinFromRegister("Nickel");
                    change.Add(nickelHolder);
                    changeValue -= 0.05;
                }
                else
                {
                    break;
                }
            }
            while (0.01 >= changeValue)
            {
                bool hasPennies = RegisterHasCoin("Penny");
                if (hasPennies == true)
                {
                    Coin quarterHolder = GetCoinFromRegister("Quarter");
                    change.Add(quarterHolder);
                    changeValue -= 0.25;
                }
                else
                {
                    return null;
                }
            }

            return change;
        }
        //Reusable method to check if the register has a coin of that name.
        //If it does have one, return true.  Else, false.
        private bool RegisterHasCoin(string name)
        {
           foreach(Coin coin in _register)
            {
                if(coin.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
        //Reusable method to return a coin from the register.
        //Returns null if no coin can be found of that name.
        private Coin GetCoinFromRegister(string name)
        {
            foreach(Coin coin in _register)
            {
                if(name == coin.Name)
                {
                    _register.Remove(coin);
                    return coin;
                }
            }
            return null;
        }
        //Takes in the total payment amount and the price of can to return the change amount.
        private double DetermineChange(double totalPayment, double canPrice)
        {
            double change = totalPayment - canPrice;
            return change;
        }
        //Takes in a list of coins to returnt he total value of the coins as a double.
        private double TotalCoinValue(List<Coin> payment)
        {
            double totalPayment = 0;
           foreach(Coin coin in payment)
            {
                totalPayment += coin.Value;
            }
            return totalPayment;
        }
        //Puts a list of coins into the soda machines register.
        private void DepositCoinsIntoRegister(List<Coin> coins)
        {
           foreach(Coin coin in coins)
            {
                _register.Add(coin);
            }
        }
    }
}
