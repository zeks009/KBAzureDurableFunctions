using KBAzureDurableFunctions.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace KBAzureDurableFunctions.Backend.Persistence;

public class RestaurantDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("restaurant.db")
            .UseSeeding((ctx, _) =>
            {
                if (!ctx.Set<MenuItem>().Any())
                {
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Focaccia", Group = "Pizza", Description = "Olivenöl, Knoblauch und Rosmarin", Price = 10.50m, Image = "https://www.funfoodfrolic.com/wp-content/uploads/2020/03/Focaccia-Thumbnail.jpg"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Margherita", Group = "Pizza", Description = "Tomatensauce, Mozzarella und Oregano", Price = 16.50m, Image = "https://images.aws.nestle.recipes/original/1821a30a8a8acec9f74a1372be582610_sem_t%C3%ADtulo_(18).jpg"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Prosciutto", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Hinterschinken und Oregano", Price = 19.50m, Image = "https://recetinas.com/wp-content/uploads/2023/07/pizza-prosciutto.jpg"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Prosciutto e funghi", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Champignon, Hinterschinken und Oregano", Price = 20.50m, Image = "https://www.ricettasprint.it/wp-content/uploads/2020/11/Pizza-alprosciutto-e-funghi-ricettasprint.jpg"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Verdura", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Saisongemu\u0308se und Oregano, Champignon, Hinterschinken und Oregano", Price = 20.50m, Image = "https://th.bing.com/th/id/OIP.0MtbBWGbpoY6vjb1MtHdgQHaE8?rs=1&pid=ImgDetMain"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Calzone V I C I N O", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Hinterschinken, Champignon, Ei und Oregano", Price = 21.50m, Image = "https://th.bing.com/th/id/R.cd311c7c82c16bcdf4d9bfeddf61f134?rik=dybS6Rq2pXDLwg&pid=ImgRaw&r=0"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Vulcano", Group = "Pizza", Description = "Tomatensauce, Mozzarella, scharfe Salami, Zwiebeln und Oregano", Price = 21.50m, Image = "https://th.bing.com/th/id/OIP.3aAD8tTHF1TajK8UTJtlUQHaG4?rs=1&pid=ImgDetMain"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Napoletana", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Kapern, Oliven, Sardellen und Oregano", Price = 21.50m, Image = "https://th.bing.com/th/id/R.1cd83bc702df3726ca572cfbd329923b?rik=EGWbsulDyrAICQ&pid=ImgRaw&r=0" });
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Quattro stagioni", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Champignon, Hinterschinken, Artischocken, Oliven und Oregano", Price = 22.50m, Image = "https://th.bing.com/th/id/OIP.OURfqhKMbB0Qa6Zf6tDBKQHaE8?rs=1&pid=ImgDetMain"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Pizza con pollo", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Pouletstreifen, roter Curry und Oregano", Price = 24.50m, Image = "https://th.bing.com/th/id/R.d36cefbf6648d765b8df6edab2efd06a?rik=h%2bKmqFnigHaCLg&pid=ImgRaw&r=0"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Parma", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Prosciutto crudo, Rucola, Parmesan und Oregano", Price = 24.50m, Image = "https://th.bing.com/th/id/OIP.GkSbL9fO0qMUHDeaSCPbswHaFj?rs=1&pid=ImgDetMain"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Dolce vita", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Olivenöl, getrocknete Tomaten, Pinienkerne, Knoblauch, Spinat und Oregano", Price = 24.50m, Image = "https://th.bing.com/th/id/R.caa636c10a53edc9e4473fc8a6bcd06b?rik=Ql1zhN0%2bfeZ7jQ&pid=ImgRaw&r=0"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Pizza Chef Art", Group = "Pizza", Description = "Tomatensauce, Rindfleischstreifen, Paprika, Zwiebeln, Cherry-Tomaten, Chili und Oregano", Price = 26.50m, Image = "https://th.bing.com/th/id/R.0632e5ef7f662f4f5cc51a6d2186ab5d?rik=OhjAxmO26mBQKw&pid=ImgRaw&r=0"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Bu\u0308ndner", Group = "Pizza", Description = "Tomatensauce, Rucola, Bu\u0308ndnerfleisch, Cherry-Tomaten und Oregano", Price = 25.50m, Image = "https://www.pizzdarija.rs/wp-content/uploads/2022/12/1670390835162209-1024x986.jpg"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Bufalina", Group = "Pizza", Description = "Tomatensauce, Bu\u0308ffel-Mozzarella, Oliven, Cherry-Tomaten, Rucola und Oregano", Price = 26.50m, Image = "https://thumbs.dreamstime.com/b/italian-pizza-bufalina-delicious-buffalo-mozzarella-tomato-sauce-basil-classic-napoli-food-103121939.jpg"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Frutti di mare", Group = "Pizza", Description = "Tomatensauce, Meeresfru\u0308chte, Cherry-Tomaten, Olivenöl und Oregano", Price = 26.50m, Image = "https://media-cdn.tripadvisor.com/media/photo-s/17/6d/48/1c/pizza-ai-frutti-di-mare.jpg"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Tonno", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Thunfisch, Oliven und Oregano", Price = 22.50m, Image = "https://blog.giallozafferano.it/ricettechepassione/wp-content/uploads/2023/03/pizza-tonno-e-cipolla-napoletanao.jpg"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Rustica", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Speck, Paprika, Zwiebeln, Knoblauch und Oregano", Price = 24.50m, Image = "https://rafael-o.uk/wp-content/uploads/2023/10/RUstica.jpg"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Mascarpone", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Paprika, Mascarpone und Oregano", Price = 20.50m, Image = "https://www.zajadam.pl/wp-content/uploads/2015/01/pizza-z-mascarpone-14.jpg"});
                    ctx.Set<MenuItem>().Add(new MenuItem(){ Name = "Pizza Royal", Group = "Pizza", Description = "Tomatensauce, Mozzarella, Schinken, Salami, Paprika, Zwiebeln und Oregano", Price = 22.50m, Image = "https://fac.img.pmdstatic.net/fit/http.3A.2F.2Fprd2-bone-image.2Es3-website-eu-west-1.2Eamazonaws.2Ecom.2Ffac.2F2018.2F07.2F30.2F509a5313-6545-4cb1-ad93-af5895dd35b6.2Ejpeg/748x372/quality/80/crop-from/center/pizza-royale.jpeg"});
                    ctx.SaveChanges();
                }
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(orders =>
        {
            orders.OwnsMany(order => order.Items);
        });
    }
}