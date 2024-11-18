using EFcore.Contexts;
using EFcore.Helpers.Exceptions;
using EFcore.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EFcore
{
    internal class Program
    {
        private static AppDbContext context = new AppDbContext();
        private static Users LoggedInUser;
        static void Main(string[] args)
        {
            //Product elave etmek:
            /*
            context.Products.Add(new Products 
            {
                Name = "Computer",
                Price ="2508.88"
            });
                 context.SaveChanges();
             */
            //Istifadeci elave etmek:
            /*
            context.Users.Add(new Users
            {
                Name = "Mehmet",
                Surname = "Ismayilov",
                Username = "Mehmet1331",
                Password = "1234m"

            });
            context.SaveChanges();
            */
            
            
            do
            {
                Console.Clear();
                Console.WriteLine("1. Register  2.Login");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    default:
                        Console.WriteLine("Duzgun secim edin");
                        break;
                }
            } while (true);
        }



        private static void Register()
        {
            Console.Clear();
            Console.WriteLine("Enter Name: ");
            var name = Console.ReadLine();
            Console.WriteLine("Enter Surname: ");
            var surname = Console.ReadLine();
            Console.WriteLine("Enter Username: ");
            var username = Console.ReadLine();
            Console.WriteLine("Enter Password: ");
            var password = Console.ReadLine();

            var user = new Users { Name = name, Surname = surname, Username = username, Password = password };
            context.Users.Add(user);
            context.SaveChanges();

            Console.WriteLine("Registration Successfully!");
            Thread.Sleep(2000);
        }

        private static void Login()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Enter Username: ");
                var username = Console.ReadLine();
                Console.WriteLine("Enter Password: ");
                var password = Console.ReadLine();

                var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (user == null)
                    throw new UserNotFoundException("User not found.");

                LoggedInUser = user;
                MainMenu();
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(2000); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(2000);
            }
        }

        private static void MainMenu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. View Basket");
                Console.WriteLine("3. Exit");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewProducts();
                        break;
                    case "2":
                        ViewBasket();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Duzgun secim edin");
                        break;
                }
            } while (true);
        }
                         
        private static void ViewProducts()
        {
            Console.Clear();
            var products = context.Products.ToList();
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id}. {product.Name} - {product.Price}");
            }

            Console.WriteLine("Enter Product ID to add to basket, go back-->Select 0 ");
            int productId;
            if (int.TryParse(Console.ReadLine(), out productId) && productId != 0)
            {
                var product = context.Products.FirstOrDefault(p => p.Id == productId);
                if (product == null)
                {
                    throw new ProductNotFoundException("Product not found.");
                }
                var basket = new Baskets { UsersId = LoggedInUser.Id, ProductsId = productId };
                context.Baskets.Add(basket);
                context.SaveChanges();
                Console.WriteLine("Product added to basket!");
            }
            Thread.Sleep(1000);
        }

        private static void ViewBasket()
        {
            try { 
            Console.Clear();
            var basketItems = context.Baskets.Where(b => b.UsersId == LoggedInUser.Id).ToList();
            if (basketItems.Count == 0)
            {
                Console.WriteLine("Your basket is empty.");
                Thread.Sleep(2000);
                return;
            }

            foreach (var item in basketItems)
            {
                var product = context.Products.FirstOrDefault(p => p.Id == item.ProductsId);
                if (product != null)
                    Console.WriteLine($"{product.Id}. {product.Name} - {product.Price}");
            }

            Console.WriteLine("Enter Product ID to remove from basket, go back-->Select 0 ");
            int productId;
            if (int.TryParse(Console.ReadLine(), out productId) && productId != 0)
            {
                var basketProduct = context.Baskets.FirstOrDefault(b => b.UsersId == LoggedInUser.Id && b.ProductsId == productId);
                if (basketProduct == null)
                {
                    throw new ProductNotFoundException("Product not found in your basket.");
                }
                context.Baskets.Remove(basketProduct);
                context.SaveChanges();
                Console.WriteLine("Product removed from basket.");
            }
            Thread.Sleep(1000);
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(2000);
            }
        }
            



    }
    
}

    
    

