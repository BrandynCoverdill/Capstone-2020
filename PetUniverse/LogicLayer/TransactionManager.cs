﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects;
using LogicLayerInterfaces;
using DataAccessLayer;
using DataAccessInterfaces;

namespace LogicLayer
{

    /// <summary>
    /// Creator: Jaeho Kim
    /// Created: 2/27/2020
    /// Approver: Rasha Mohammed
    /// 
    /// Manager Class for Transactions
    /// </summary>
    public class TransactionManager : ITransactionManager
    {
        private ITransactionAccessor _transactionAccessor;

        /// <summary>
		/// Creator: Jaeho Kim
		/// Created: 2/27/2020
		/// Approver: Rasha Mohammed
		/// 
		/// Default Constructor for instantiating Accessor
		/// </summary>
		/// <remarks>
		/// Updater: NA
		/// Updated: NA
		/// Update: NA
		/// </remarks>
		public TransactionManager()
        {
            _transactionAccessor = new TransactionAccessor();
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 2/26/2020
        /// Approver: Rasha Mohammed
        /// 
        /// Constructor for passing specific Accessor class
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// 
        /// </remarks>
        /// <param name="transactionAccessor"></param>

        public TransactionManager(ITransactionAccessor transactionAccessor)
        {
            _transactionAccessor = transactionAccessor;
        }

        // This is the list of all products.
        List<ProductVM> ShoppingCart = new List<ProductVM>();

        List<TransactionLineProducts> TransactionLineProductsList = new List<TransactionLineProducts>();

        // This is the list of taxable products only.
        List<ProductVM> TaxableProductsInShoppingCart = new List<ProductVM>();

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of AddProduct method. Adds the product 
        /// to the ShoppingCart List.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        public void AddProduct(ProductVM productVM)
        {
            try
            {
                ShoppingCart.Add(productVM);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could Not Add Product.", ex);
            }
        }

        /// <summary>
        /// CREATOR: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of AddProductTaxable method. Adds the product 
        /// to the ShoppingCartTaxable List.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        public void AddProductTaxable(ProductVM productVM)
        {
            try
            {
                TaxableProductsInShoppingCart.Add(productVM);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could Not Add Product.", ex);
            }
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of AddTransaction method. 
        /// Adds the transaction.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        public bool AddTransaction(Transaction transaction)
        {
            bool result = false;

            try
            {
                result = (_transactionAccessor.InsertTransaction(transaction) > 0);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could Not Add Transaction.", ex);
            }
            return result;
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 04/04/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of AddTransactionLineProducts method. 
        /// Adds the transaction.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>bool</returns>
        public bool AddTransactionLineProducts(TransactionLineProducts transactionLineProducts)
        {
            bool result = false;

            try
            {
                result = (_transactionAccessor.InsertTransactionLineProducts(transactionLineProducts) > 0);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could Not Add Transaction.", ex);
            }
            return result;
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of CalculateSubTotal method. 
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>decimal</returns>
        public decimal CalculateSubTotal(List<ProductVM> AllProductsList)
        {
            decimal subTotal = 0.00M;
            foreach (var item in AllProductsList)
            {
                subTotal = subTotal + item.Price;
            }
            return subTotal;
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of CalculateSubTotalTaxable method. 
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>decimal</returns>
        public decimal CalculateSubTotalTaxable(List<ProductVM> TaxableProductList)
        {
            decimal subTotalTaxable = 0.00M;
            foreach (var item in TaxableProductList)
            {
                subTotalTaxable = subTotalTaxable + item.Price;
            }
            return subTotalTaxable;
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/22/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of CalculateSubTotal method. 
        /// The subTotalTaxable is multiplied by the 
        /// salesTax rate. This number is then added 
        /// to the subTotal. After this calculation, 
        /// the total is yielded.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>decimal</returns>
        public decimal CalculateTotal(decimal subTotal, decimal subTotalTaxable, SalesTax salesTax)
        {
            decimal tax = subTotalTaxable * salesTax.TaxRate;

            decimal total = subTotal + tax;

            return total;
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 04/04/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of clear shopping cart method. 
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        public void ClearShoppingCart()
        {
            ShoppingCart.Clear();
            TaxableProductsInShoppingCart.Clear();
        }

        /// <summary>
        /// NAME: Rasha Mohammed
        /// DATE: 2/21/2020
        /// CHECKED BY: Jaeho Kim
        /// 
        /// Method that delete item.
        /// </summary>
        /// <remarks>
        /// UPDATED BY:
        /// UPDATED DATE: 
        /// CHANGES: 
        /// </remarks>
        public bool DeleteItem(string productID)
        {
            bool result = false;
            try
            {
                result = (1 == _transactionAccessor.DeleteItemFromTransaction(productID));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Item not removed!", ex);
            }
            return result;
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of EnumerableAllProducts method. This implementation 
        /// loads all of the products in the shopping cart. Then the lambda 
        /// expression is used to check for any duplicate product IDs. If 
        /// there is, the item quantity is added together, and the sum is 
        /// assigned to the new product id of the item quantity.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>AllProducts</returns>
        public IEnumerable<ProductVM> EnumerableAllProducts()
        {
            List<ProductVM> AllProducts = new List<ProductVM>();

            foreach (var item in ShoppingCart)
            {
                AllProducts.Add(item);
            }

            return AllProducts.GroupBy(o => o.ProductID)
                              .Select(g => new ProductVM
                              {
                                  ProductID = g.Key,
                                  Quantity = g.Sum(o => o.Quantity),
                                  Price = g.Sum(o => o.Price * o.Quantity)
                              });
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of EnumerableTaxableProducts method. This implementation 
        /// loads the taxable products in the shopping cart taxable list. Then 
        /// the lambda expression is used to check for any duplicate product IDs. 
        /// If there is, the item quantity is added together, and the sum is 
        /// assigned to the new product id of the item quantity.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>TaxableProducts</returns>
        public IEnumerable<ProductVM> EnumerableTaxableProducts()
        {
            List<ProductVM> TaxableProducts = new List<ProductVM>();
            foreach (var item in TaxableProductsInShoppingCart)
            {
                TaxableProducts.Add(item);
            }

            return TaxableProducts.GroupBy(o => o.ProductID)
                              .Select(g => new ProductVM
                              {
                                  ProductID = g.Key,
                                  Quantity = g.Sum(o => o.Quantity),
                                  Price = g.Sum(o => o.Price * o.Quantity),
                              });
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of GetAllProducts method. This implementation 
        /// gets the IEnumerable and turns it into an ArrayList of AllProducts.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>AllProducts</returns>
        public List<ProductVM> GetAllProducts()
        {
            // The IEnumerable takes the list of all products. If the list of all products 
            // have multiple product UPC, the lambda expression conducts an operation where 
            // it takes the item quantity of duplicate product UPCs and adds them together.
            // Once sum is obtained, the sum is assigned to the itemQuantity, of the 
            // product UPC.
            IEnumerable<ProductVM> AllProductVMs = EnumerableAllProducts();
            List<ProductVM> ShoppingCart = AllProductVMs.ToList();

            return ShoppingCart;
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/22/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of GetTaxableProducts method. This implementation 
        /// gets the IEnumerable and turns it into an ArrayList of TaxableProducts.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>AllProducts</returns>
        public List<ProductVM> GetTaxableProducts()
        {
            // The IEnumerable takes the list of taxable products. If the list of taxable products 
            // have multiple product UPC, the lambda expression conducts an operation where 
            // it takes the item quantity of duplicate product UPCs and adds them together.
            // Once sum is obtained, the sum is assigned to the itemQuantity, of the 
            // product UPC.
            IEnumerable<ProductVM> TaxableProductVMs = EnumerableTaxableProducts();
            List<ProductVM> TaxableProductsInShoppingCart = TaxableProductVMs.ToList();

            return TaxableProductsInShoppingCart;
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/22/2020
        /// Approver: Rasha Mohammed
        ///
        /// Validates and makes sure the quantity in the shopping cart does 
        /// not exceed the quantity in stock(item quantity).
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>bool</returns>
        public bool isItemQuantityValid(List<ProductVM> AllProductVMs, ProductVM productVM)
        {
            bool isValid = false;
            try
            {
                // First, the logic must check if there is already a list of products. This 
                // is done ONLY to check if there is any duplicate product UPCs. If there is, 
                // the logic must CHECK that the product item quantity (that's already in 
                // the list) added to the product item quantity that's being added, must 
                // NOT EXCEED the quantity in stock. THE LOGIC BELOW ONLY CHECKS AND 
                // VERIFIES THE PROPERTY VALUES!!!
                if (AllProductVMs.Count != 0)
                {

                    bool isDuplicate = false;

                    // This value represents the item quantity that's already 
                    // in the list, if duplicate product UPCs are found.
                    int itemQuantity = 0;

                    // Search through all of the items in the master product list.
                    foreach (var item in AllProductVMs)
                    {

                        if (item.ProductID == productVM.ProductID)
                        {
                            // Duplicate Product has been found
                            itemQuantity = item.Quantity;
                            isDuplicate = true;
                        }
                    }
                    if (isDuplicate)
                    {
                        // add the sum of BOTH item quantities.
                        int totalQuantity = itemQuantity + productVM.Quantity;

                        // verify that it doesn't exceed the quantity in stock.
                        // also verifies the QUANTITY ENTERED is not zero or less.
                        if (totalQuantity <= productVM.ItemQuantity && productVM.Quantity > 0)
                        {
                            isValid = true;
                        }
                    }

                    // This else statement runs whenever a DIFFERENT product is added.
                    else
                    {
                        if (productVM.Quantity <= productVM.ItemQuantity && productVM.Quantity > 0)
                        {
                            isValid = true;
                        }
                    }

                }

                // If the program is adding the product to the shopping cart for the 
                // FIRST TIME ONLY, all the logic needs to do is make sure the quantity 
                // that's being added does not exceed the quantity in stock.
                // THIS BLOCK OF CODE SHOULD RUN ONLY ONCE!!!
                else
                {
                    if (productVM.Quantity <= productVM.ItemQuantity && productVM.Quantity > 0)
                    {
                        isValid = true;
                    }

                }
                return isValid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 3/05/2020
        /// Approver: Rasha Mohammed
        /// 
        /// This method calls the Select all products with transaction id method in the DataAccessLayer.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>TransactionVM list</returns>
        public List<TransactionVM> RetrieveAllProductsByTransactionID(int transactionID)
        {
            try
            {
                return _transactionAccessor.SelectAllProductsByTransactionID(transactionID);
            }

            catch (Exception ex)
            {
                throw new ApplicationException("Products Not Found", ex);
            }
        }


        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of RetrieveLatestSalesTaxDateByZipCode method. 
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>SalesTax Date</returns>
        public DateTime RetrieveLatestSalesTaxDateByZipCode(string zipCode)
        {
            try
            {
                return _transactionAccessor.SelectLatestSalesTaxDateByZipCode(zipCode);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found.", ex);
            }
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of RetrieveProductByProductID method. 
        /// This implementation is used to populate productVM 
        /// details using the productID.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>ProductVM</returns>
        public ProductVM RetrieveProductByProductID(string productID)
        {
            try
            {
                return _transactionAccessor.SelectProductByProductID(productID);
            }

            catch (Exception ex)
            {
                throw new ApplicationException("Product Not Found", ex);
            }
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 03/19/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of RetrieveTaxRateBySalesTaxDateAndZipCode method. 
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>TaxRate</returns>
        public decimal RetrieveTaxRateBySalesTaxDateAndZipCode(string zipCode, DateTime salesTaxDate)
        {
            try
            {
                return _transactionAccessor.SelectTaxRateBySalesTaxDateAndZipCode(zipCode, salesTaxDate);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found.", ex);
            }
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 04/04/2020
        /// Approver: Rasha Mohammed
        ///
        /// Implementation of RetrieveTransactionByTransactionDate method. 
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>TransactionVM List</returns>
        public List<TransactionVM> RetrieveTransactionByTransactionDate(DateTime transactionDate)
        {
            try
            {
                return _transactionAccessor.SelectTransactionsByTransactionDate(transactionDate);
            }

            catch (Exception ex)
            {
                throw new ApplicationException("Transactions Not Found", ex);
            }
        }

        /// <summary>
        /// Creator: Jaeho Kim
        /// Created: 3/08/2020
        /// Approver: Rasha Mohammed
        ///  
        /// This method calls the Select transactions by name method in the DataAccessLayer.
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <param name="firstName">Employee first name</param>
        /// <param name="lastName">Employee last name</param>
        /// <returns>TransactionVM List</returns>
        public List<TransactionVM> RetrieveTransactionByEmployeeName(string firstName, string lastName)
        {
            try
            {
                return _transactionAccessor.SelectTransactionsByEmployeeName(firstName, lastName);
            }

            catch (Exception ex)
            {
                throw new ApplicationException("Transactions Not Found", ex);
            }
        }

        /// <summary>
        /// CREATOR: Rashs Mohammed
        /// CREATED: 4/11/2020
        /// APPROVER: Robert Holmes
        ///
        /// Implementation of EditProduct method that edit the price on transaction. 
        /// 
        /// </summary>
        /// <remarks>
        /// Updater: NA
        /// Updated: NA
        /// Update: NA
        /// </remarks>
        /// <returns>bool</returns>
        public bool EditProduct(ProductVM oldProduct, ProductVM newProduct)
        {
            bool result = false;

            try
            {
                result = true;
                
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed", ex);
            }

            return result;
        }
    }
}
