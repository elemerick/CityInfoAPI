namespace PizzaStore
{
    public record Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class PizzaDB
    {
        public static List<Pizza> _pizza = new List<Pizza>()
        {
            new Pizza { Id = 1, Name = "pizza 1" },
            new Pizza { Id = 2, Name = "pizza 2" },
            new Pizza { Id = 3, Name = "pizza 3" }
        };

        public static List<Pizza> GetPizza()
        {
            return _pizza;
        }

        public static Pizza ? GetPizza(int id) { 
            return _pizza.SingleOrDefault(p => p.Id == id);
        }

        public static Pizza CreatePizza(Pizza pizza) {
            _pizza.Add(pizza);
            return pizza;

        }

        public static void UpdatePizza(Pizza pizza) { 
            _pizza = _pizza.Select(p => p.Id == pizza.Id ? new Pizza { Id = p.Id, Name = pizza.Name } : p).ToList();
        }

        public static void DeletePizza(int id) { 
            _pizza = _pizza.FindAll(p => p.Id != id);
        }
    }
}
