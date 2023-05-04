using BreweryAPI.Controllers;
using BreweryAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace BreweryAPI
{
    public class BreweryHub : Hub
    {
        public async Task AuthorizationAdmin(string PhoneAdmin, string PasswordAdmin)
        {
            AdminsController controller = new AdminsController(new BreweryContext());

            var token = controller.Authorization(PhoneAdmin, PasswordAdmin).Result.Value;

            await this.Clients.All.SendAsync("auth", token);
        }
        public async Task AuthorizationWithKeyAdmin(string login, string authKey)
        {
            AdminsController controller = new AdminsController(new BreweryContext());

            var token = controller.GetAuthentication(login, authKey).Result.Value;

            await this.Clients.All.SendAsync("authWithKey", token);
        }
        public async Task GetAuthKeyAdmin(string login)
        {
            AdminsController controller = new AdminsController(new BreweryContext());

            var key = controller.GetAuthKey(login).Result.Value;

            await this.Clients.All.SendAsync("getKey", key);
        }

        //---------------------------------------------Get----------------------------------------------------------
        public async Task GetAdmins()
        {
            AdminsController controller = new AdminsController(new BreweryContext());

            var result = controller.GetAdmins().Result.Value;

            await this.Clients.All.SendAsync("getAdmins", result);
        }
        public async Task GetAdminsLists()
        {
            AdminsListsController controller = new AdminsListsController(new BreweryContext());

            var result = controller.GetAdminsLists().Result.Value;

            await this.Clients.All.SendAsync("getAdminsLists", result);
        }
        public async Task GetBeerCheques()
        {
            BeerChequesController controller = new BeerChequesController(new BreweryContext());

            var result = controller.GetBeerCheques().Result.Value;

            await this.Clients.All.SendAsync("getBeerCheques", result);
        }
        public async Task GetBeers()
        {
            BeersController controller = new BeersController(new BreweryContext());

            var result = controller.GetBeers().Result.Value;

            await this.Clients.All.SendAsync("getBeers", result);
        }
        public async Task GetBeersLists()
        {
            BeersListsController controller = new BeersListsController(new BreweryContext());

            var result = controller.GetBeersLists().Result.Value;

            await this.Clients.All.SendAsync("getBeersLists", result);
        }
        public async Task GetBeerTypes()
        {
            BeerTypesController controller = new BeerTypesController(new BreweryContext());

            var result = controller.GetBeerTypes().Result.Value;

            await this.Clients.All.SendAsync("getBeerTypes", result);
        }
        public async Task GetBreweries()
        {
            BreweriesController controller = new BreweriesController(new BreweryContext());

            var result = controller.GetBreweries().Result.Value;

            await this.Clients.All.SendAsync("getBreweries", result);
        }
        public async Task GetBreweriesLists()
        {
            BreweriesListsController controller = new BreweriesListsController(new BreweryContext());

            var result = controller.GetBreweriessLists().Result.Value;

            await this.Clients.All.SendAsync("getBreweriesLists", result);
        }
        public async Task GetBreweryBeers()
        {
            BreweryBeersController controller = new BreweryBeersController(new BreweryContext());

            var result = controller.GetBreweryBeers().Result.Value;

            await this.Clients.All.SendAsync("getBreweryBeers", result);
        }
        public async Task GetBreweryIngridients()
        {
            BreweryIngridientsController controller = new BreweryIngridientsController(new BreweryContext());

            var result = controller.GetBreweryIngridients().Result.Value;

            await this.Clients.All.SendAsync("getBreweryIngridients", result);
        }
        public async Task GetCheques()
        {
            ChequesController controller = new ChequesController(new BreweryContext());

            var result = controller.GetCheques().Result.Value;

            await this.Clients.All.SendAsync("getCheques", result);
        }
        public async Task GetChequesLists()
        {
            ChequesListsController controller = new ChequesListsController(new BreweryContext());

            var result = controller.GetChequesLists().Result.Value;

            await this.Clients.All.SendAsync("getChequesLists", result);
        }
        public async Task GetIngridients()
        {
            IngridientsController controller = new IngridientsController(new BreweryContext());

            var result = controller.GetIngridients().Result.Value;

            await this.Clients.All.SendAsync("getIngridients", result);
        }
        public async Task GetIngridientsBeers()
        {
            IngridientsBeersController controller = new IngridientsBeersController(new BreweryContext());

            var result = controller.GetIngridientsBeers().Result.Value;

            await this.Clients.All.SendAsync("getIngridientsBeers", result);
        }
        public async Task GetIngridientsLists()
        {
            IngridientsListsController controller = new IngridientsListsController(new BreweryContext());

            var result = controller.GetIngridientsLists().Result.Value;

            await this.Clients.All.SendAsync("getIngridientsLists", result);
        }
        public async Task GetIngridientsTypes()
        {
            IngridientsTypesController controller = new IngridientsTypesController(new BreweryContext());

            var result = controller.GetIngridientsTypes().Result.Value;

            await this.Clients.All.SendAsync("getIngridientsTypes", result);
        }
        public async Task GetSubscriptions()
        {
            SubscriptionsController controller = new SubscriptionsController(new BreweryContext());

            var result = controller.GetSubscriptions().Result.Value;

            await this.Clients.All.SendAsync("getSubscriptions", result);
        }
        public async Task GetSuppliers()
        {
            SuppliersController controller = new SuppliersController(new BreweryContext());

            var result = controller.GetSuppliers().Result.Value;

            await this.Clients.All.SendAsync("getSuppliers", result);
        }
        public async Task GetSuppliersLists()
        {
            SuppliersListsController controller = new SuppliersListsController(new BreweryContext());

            var result = controller.GetSuppliersLists().Result.Value;

            await this.Clients.All.SendAsync("getSuppliersLists", result);
        }
        public async Task GetUsers()
        {
            UsersController controller = new UsersController(new BreweryContext());

            var result = controller.GetUsers().Result.Value;

            await this.Clients.All.SendAsync("getUsers", result);
        }
        public async Task GetUsersLists()
        {
            UsersListsController controller = new UsersListsController(new BreweryContext());

            var result = controller.GetUsersLists().Result.Value;

            await this.Clients.All.SendAsync("getUsersLists", result);
        }

        //---------------------------------------------Post----------------------------------------------------------
        public async Task PostAdmin(Admin? admin)
        {
            AdminsController controller = new AdminsController(new BreweryContext());

            controller.PostAdmin(admin);

            await this.Clients.All.SendAsync("postAdmin");
        }
        public async Task PostBeerCheque(BeerCheque? beerCheque)
        {
            BeerChequesController controller = new BeerChequesController(new BreweryContext());

            controller.PostBeerCheque(beerCheque);

            await this.Clients.All.SendAsync("postBeerCheque");
        }
        public async Task PostBeer(Beer? beer)
        {
            BeersController controller = new BeersController(new BreweryContext());

            controller.PostBeer(beer);

            await this.Clients.All.SendAsync("postBeer");
        }
        public async Task PostBeerType(BeerType? beerType)
        {
            BeerTypesController controller = new BeerTypesController(new BreweryContext());

            controller.PostBeerType(beerType);

            await this.Clients.All.SendAsync("postBeerType");
        }
        public async Task PostBreweries(Brewery? brewery)
        {
            BreweriesController controller = new BreweriesController(new BreweryContext());

            controller.PostBrewery(brewery);

            await this.Clients.All.SendAsync("postBrewery");
        }
        public async Task PostBreweryBeer(BreweryBeer? breweryBeer)
        {
            BreweryBeersController controller = new BreweryBeersController(new BreweryContext());

            controller.PostBreweryBeer(breweryBeer);

            await this.Clients.All.SendAsync("postBreweryBeer");
        }
        public async Task PostBreweryIngridient(BreweryIngridient? breweryIngridient)
        {
            BreweryIngridientsController controller = new BreweryIngridientsController(new BreweryContext());

            controller.PostBreweryIngridient(breweryIngridient);

            await this.Clients.All.SendAsync("postBreweryIngridient");
        }
        public async Task PostCheque(Cheque? cheque)
        {
            ChequesController controller = new ChequesController(new BreweryContext());

            controller.PostCheque(cheque);

            await this.Clients.All.SendAsync("postCheque");
        }
        public async Task PostIngridient(Ingridient? ingridient)
        {
            IngridientsController controller = new IngridientsController(new BreweryContext());

            controller.PostIngridient(ingridient);

            await this.Clients.All.SendAsync("postIngridient");
        }
        public async Task PostIngridientsBeer(IngridientsBeer? ingridientsBeer)
        {
            IngridientsBeersController controller = new IngridientsBeersController(new BreweryContext());

            controller.PostIngridientsBeer(ingridientsBeer);

            await this.Clients.All.SendAsync("postIngridientsBeer");
        }
        public async Task PostIngridientsType(IngridientsType? ingridientsType)
        {
            IngridientsTypesController controller = new IngridientsTypesController(new BreweryContext());

            controller.PostIngridientsType(ingridientsType);

            await this.Clients.All.SendAsync("postIngridientsType");
        }
        public async Task PostSubscription(Subscription? subscription)
        {
            SubscriptionsController controller = new SubscriptionsController(new BreweryContext());

            controller.PostSubscription(subscription);

            await this.Clients.All.SendAsync("postSubscription");
        }
        public async Task PostSupplier(Supplier? supplier)
        {
            SuppliersController controller = new SuppliersController(new BreweryContext());

            controller.PostSupplier(supplier);

            await this.Clients.All.SendAsync("postSupplier");
        }
        public async Task PostUser(User? user)
        {
            UsersController controller = new UsersController(new BreweryContext());

            controller.PostUser(user);

            await this.Clients.All.SendAsync("postUser");
        }

        //---------------------------------------------Put----------------------------------------------------------
        public async Task PutAdmin(int id, Admin? admin)
        {
            AdminsController controller = new AdminsController(new BreweryContext());

            controller.PutAdmin(id, admin);

            await this.Clients.All.SendAsync("putAdmin");
        }
        public async Task PutBeerCheque(int id, BeerCheque? beerCheque)
        {
            BeerChequesController controller = new BeerChequesController(new BreweryContext());

            controller.PutBeerCheque(id, beerCheque);

            await this.Clients.All.SendAsync("putBeerCheque");
        }
        public async Task PutBeer(int id, Beer? beer)
        {
            BeersController controller = new BeersController(new BreweryContext());

            controller.PutBeer(id, beer);

            await this.Clients.All.SendAsync("putBeer");
        }
        public async Task PutBeerType(int id, BeerType? beerType)
        {
            BeerTypesController controller = new BeerTypesController(new BreweryContext());

            controller.PutBeerType(id, beerType);

            await this.Clients.All.SendAsync("putBeerType");
        }
        public async Task PutBreweries(int id, Brewery? brewery)
        {
            BreweriesController controller = new BreweriesController(new BreweryContext());

            controller.PutBrewery(id, brewery);

            await this.Clients.All.SendAsync("putBrewery");
        }
        public async Task PutBreweryBeer(int id, BreweryBeer? breweryBeer)
        {
            BreweryBeersController controller = new BreweryBeersController(new BreweryContext());

            controller.PutBreweryBeer(id, breweryBeer);

            await this.Clients.All.SendAsync("putBreweryBeer");
        }
        public async Task PutBreweryIngridient(int id, BreweryIngridient? breweryIngridient)
        {
            BreweryIngridientsController controller = new BreweryIngridientsController(new BreweryContext());

            controller.PutBreweryIngridient(id, breweryIngridient);

            await this.Clients.All.SendAsync("putBreweryIngridient");
        }
        public async Task PutCheque(int id, Cheque? cheque)
        {
            ChequesController controller = new ChequesController(new BreweryContext());

            controller.PutCheque(id, cheque);

            await this.Clients.All.SendAsync("putCheque");
        }
        public async Task PutIngridient(int id, Ingridient? ingridient)
        {
            IngridientsController controller = new IngridientsController(new BreweryContext());

            controller.PutIngridient(id, ingridient);

            await this.Clients.All.SendAsync("putIngridient");
        }
        public async Task PutIngridientsBeer(int id, IngridientsBeer? ingridientsBeer)
        {
            IngridientsBeersController controller = new IngridientsBeersController(new BreweryContext());

            controller.PutIngridientsBeer(id, ingridientsBeer);

            await this.Clients.All.SendAsync("putIngridientsBeer");
        }
        public async Task PutIngridientsType(int id, IngridientsType? ingridientsType)
        {
            IngridientsTypesController controller = new IngridientsTypesController(new BreweryContext());

            controller.PutIngridientsType(id, ingridientsType);

            await this.Clients.All.SendAsync("putIngridientsType");
        }
        public async Task PutSubscription(int id, Subscription? subscription)
        {
            SubscriptionsController controller = new SubscriptionsController(new BreweryContext());

            controller.PutSubscription(id, subscription);

            await this.Clients.All.SendAsync("putSubscription");
        }
        public async Task PutSupplier(int id, Supplier? supplier)
        {
            SuppliersController controller = new SuppliersController(new BreweryContext());

            controller.PutSupplier(id, supplier);

            await this.Clients.All.SendAsync("putSupplier");
        }
        public async Task PutUser(int id, User? user)
        {
            UsersController controller = new UsersController(new BreweryContext());

            controller.PutUser(id, user);

            await this.Clients.All.SendAsync("putUser");
        }

        //---------------------------------------------Delete----------------------------------------------------------
        public async Task DeleteAdmin(int id)
        {
            AdminsController controller = new AdminsController(new BreweryContext());

            controller.DeleteAdmin(id);

            await this.Clients.All.SendAsync("deleteAdmin");
        }
        public async Task DeleteBeerCheque(int id)
        {
            BeerChequesController controller = new BeerChequesController(new BreweryContext());

            controller.DeleteBeerCheque(id);

            await this.Clients.All.SendAsync("deleteBeerCheque");
        }
        public async Task DeleteBeer(int id)
        {
            BeersController controller = new BeersController(new BreweryContext());

            controller.DeleteBeer(id);

            await this.Clients.All.SendAsync("deleteBeer");
        }
        public async Task DeleteBeerType(int id)
        {
            BeerTypesController controller = new BeerTypesController(new BreweryContext());

            controller.DeleteBeerType(id);

            await this.Clients.All.SendAsync("deleteBeerType");
        }
        public async Task DeleteBreweries(int id)
        {
            BreweriesController controller = new BreweriesController(new BreweryContext());

            controller.DeleteBrewery(id);

            await this.Clients.All.SendAsync("deleteBrewery");
        }
        public async Task DeleteBreweryBeer(int id)
        {
            BreweryBeersController controller = new BreweryBeersController(new BreweryContext());

            controller.DeleteBreweryBeer(id);

            await this.Clients.All.SendAsync("deleteBreweryBeer");
        }
        public async Task DeleteBreweryIngridient(int id)
        {
            BreweryIngridientsController controller = new BreweryIngridientsController(new BreweryContext());

            controller.DeleteBreweryIngridient(id);

            await this.Clients.All.SendAsync("deleteBreweryIngridient");
        }
        public async Task DeleteCheque(int id)
        {
            ChequesController controller = new ChequesController(new BreweryContext());

            controller.DeleteCheque(id);

            await this.Clients.All.SendAsync("deleteCheque");
        }
        public async Task DeleteIngridient(int id)
        {
            IngridientsController controller = new IngridientsController(new BreweryContext());

            controller.DeleteIngridient(id);

            await this.Clients.All.SendAsync("deleteIngridient");
        }
        public async Task DeleteIngridientsBeer(int id)
        {
            IngridientsBeersController controller = new IngridientsBeersController(new BreweryContext());

            controller.DeleteIngridientsBeer(id);

            await this.Clients.All.SendAsync("deleteIngridientsBeer");
        }
        public async Task DeleteIngridientsType(int id)
        {
            IngridientsTypesController controller = new IngridientsTypesController(new BreweryContext());

            controller.DeleteIngridientsType(id);

            await this.Clients.All.SendAsync("deleteIngridientsType");
        }
        public async Task DeleteSubscription(int id)
        {
            SubscriptionsController controller = new SubscriptionsController(new BreweryContext());

            controller.DeleteSubscription(id);

            await this.Clients.All.SendAsync("deleteSubscription");
        }
        public async Task DeleteSupplier(int id)
        {
            SuppliersController controller = new SuppliersController(new BreweryContext());

            controller.DeleteSupplier(id);

            await this.Clients.All.SendAsync("deleteSupplier");
        }
        public async Task DeleteUser(int id)
        {
            UsersController controller = new UsersController(new BreweryContext());

            controller.DeleteUser(id);

            await this.Clients.All.SendAsync("deleteUser");
        }

        //---------------------------------------------LogicalDelete----------------------------------------------------------
        public async Task LogicalDeleteAdmin(int[] id)
        {
            AdminsController controller = new AdminsController(new BreweryContext());

            controller.LogicalDeleteAdmin(id);

            await this.Clients.All.SendAsync("logicalDeleteAdmin");
        }
        public async Task LogicalDeleteBeerCheque(int[] id)
        {
            BeerChequesController controller = new BeerChequesController(new BreweryContext());

            controller.LogicalDeleteBeerCheque(id);

            await this.Clients.All.SendAsync("logicalDeleteBeerCheque");
        }
        public async Task LogicalDeleteBeer(int[] id)
        {
            BeersController controller = new BeersController(new BreweryContext());

            controller.LogicalDeleteBeer(id);

            await this.Clients.All.SendAsync("logicalDeleteBeer");
        }
        public async Task LogicalDeleteBeerType(int[] id)
        {
            BeerTypesController controller = new BeerTypesController(new BreweryContext());

            controller.LogicalDeleteBeerType(id);

            await this.Clients.All.SendAsync("logicalDeleteBeerType");
        }
        public async Task LogicalDeleteBreweries(int[] id)
        {
            BreweriesController controller = new BreweriesController(new BreweryContext());

            controller.LogicalDeleteBrewery(id);

            await this.Clients.All.SendAsync("logicalDeleteBrewery");
        }
        public async Task LogicalDeleteBreweryBeer(int[] id)
        {
            BreweryBeersController controller = new BreweryBeersController(new BreweryContext());

            controller.LogicalDeleteBreweryBeer(id);

            await this.Clients.All.SendAsync("logicalDeleteBreweryBeer");
        }
        public async Task LogicalDeleteBreweryIngridient(int[] id)
        {
            BreweryIngridientsController controller = new BreweryIngridientsController(new BreweryContext());

            controller.LogicalDeleteBreweryIngridient(id);

            await this.Clients.All.SendAsync("logicalDeleteBreweryIngridient");
        }
        public async Task LogicalDeleteCheque(int[] id)
        {
            ChequesController controller = new ChequesController(new BreweryContext());

            controller.LogicalDeleteCheque(id);

            await this.Clients.All.SendAsync("logicalDeleteCheque");
        }
        public async Task LogicalDeleteIngridient(int[] id)
        {
            IngridientsController controller = new IngridientsController(new BreweryContext());

            controller.LogicalDeleteIngridients(id);

            await this.Clients.All.SendAsync("logicalDeleteIngridient");
        }
        public async Task LogicalDeleteIngridientsBeer(int[] id)
        {
            IngridientsBeersController controller = new IngridientsBeersController(new BreweryContext());

            controller.LogicalDeleteIngridientsBeer(id);

            await this.Clients.All.SendAsync("logicalDeleteIngridientsBeer");
        }
        public async Task LogicalDeleteIngridientsType(int[] id)
        {
            IngridientsTypesController controller = new IngridientsTypesController(new BreweryContext());

            controller.LogicalDeleteIngridientsTypes(id);

            await this.Clients.All.SendAsync("logicalDeleteIngridientsType");
        }
        public async Task LogicalDeleteSubscription(int[] id)
        {
            SubscriptionsController controller = new SubscriptionsController(new BreweryContext());

            controller.LogicalDeleteSubscription(id);

            await this.Clients.All.SendAsync("logicalDeleteSubscription");
        }
        public async Task LogicalDeleteSupplier(int[] id)
        {
            SuppliersController controller = new SuppliersController(new BreweryContext());

            controller.LogicalDeleteSupplier(id);

            await this.Clients.All.SendAsync("logicalDeleteSupplier");
        }
        public async Task LogicalDeleteUser(int[] id)
        {
            UsersController controller = new UsersController(new BreweryContext());

            controller.LogicalDeleteUser(id);

            await this.Clients.All.SendAsync("logicalDeleteUser");
        }

        //---------------------------------------------LogicalRestore----------------------------------------------------------
        public async Task LogicalRestoreAdmin(int[] id)
        {
            AdminsController controller = new AdminsController(new BreweryContext());

            controller.LogicalRestoreAdmin(id);

            await this.Clients.All.SendAsync("logicalRestoreAdmin");
        }
        public async Task LogicalRestoreBeerCheque(int[] id)
        {
            BeerChequesController controller = new BeerChequesController(new BreweryContext());

            controller.LogicRestoreBeerCheque(id);

            await this.Clients.All.SendAsync("logicalRestoreBeerCheque");
        }
        public async Task LogicalRestoreBeer(int[] id)
        {
            BeersController controller = new BeersController(new BreweryContext());

            controller.LogicalRestoreBeer(id);

            await this.Clients.All.SendAsync("logicalRestoreBeer");
        }
        public async Task LogicalRestoreBeerType(int[] id)
        {
            BeerTypesController controller = new BeerTypesController(new BreweryContext());

            controller.LogicalRestoreBeerType(id);

            await this.Clients.All.SendAsync("logicalRestoreBeerType");
        }
        public async Task LogicalRestoreBreweries(int[] id)
        {
            BreweriesController controller = new BreweriesController(new BreweryContext());

            controller.LogicalRestoreBrewery(id);

            await this.Clients.All.SendAsync("logicalRestoreBrewery");
        }
        public async Task LogicalRestoreBreweryBeer(int[] id)
        {
            BreweryBeersController controller = new BreweryBeersController(new BreweryContext());

            controller.LogicalRestoreBreweryBeer(id);

            await this.Clients.All.SendAsync("logicalRestoreBreweryBeer");
        }
        public async Task LogicalRestoreBreweryIngridient(int[] id)
        {
            BreweryIngridientsController controller = new BreweryIngridientsController(new BreweryContext());

            controller.LogicalRestoreBreweryIngridient(id);

            await this.Clients.All.SendAsync("logicalRestoreBreweryIngridient");
        }
        public async Task LogicalRestoreCheque(int[] id)
        {
            ChequesController controller = new ChequesController(new BreweryContext());

            controller.LogicalRestoreCheque(id);

            await this.Clients.All.SendAsync("logicalRestoreCheque");
        }
        public async Task LogicalRestoreIngridient(int[] id)
        {
            IngridientsController controller = new IngridientsController(new BreweryContext());

            controller.LogicalRestoreIngridients(id);

            await this.Clients.All.SendAsync("logicalRestoreIngridient");
        }
        public async Task LogicalRestoreIngridientsBeer(int[] id)
        {
            IngridientsBeersController controller = new IngridientsBeersController(new BreweryContext());

            controller.LogicalRestoreIngridientsBeer(id);

            await this.Clients.All.SendAsync("logicalRestoreIngridientsBeer");
        }
        public async Task LogicalRestoreIngridientsType(int[] id)
        {
            IngridientsTypesController controller = new IngridientsTypesController(new BreweryContext());

            controller.LogicalRestoreIngridientsTypes(id);

            await this.Clients.All.SendAsync("logicalRestoreIngridientsType");
        }
        public async Task LogicalRestoreSubscription(int[] id)
        {
            SubscriptionsController controller = new SubscriptionsController(new BreweryContext());

            controller.LogicalRestoreSubscription(id);

            await this.Clients.All.SendAsync("logicalRestoreSubscription");
        }
        public async Task LogicalRestoreSupplier(int[] id)
        {
            SuppliersController controller = new SuppliersController(new BreweryContext());

            controller.LogicalRestoreSupplier(id);

            await this.Clients.All.SendAsync("logicalRestoreSupplier");
        }
        public async Task LogicalRestoreUser(int[] id)
        {
            UsersController controller = new UsersController(new BreweryContext());

            controller.LogicalRestoreUser(id);

            await this.Clients.All.SendAsync("logicalRestoreUser");
        }
    }
}
