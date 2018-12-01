# **Supply BlockChain Xamarin.Forms App**

## **Overview**

**Supply BlockChain** is a system that implements a manufacturing supply chain in the form of a blockchain. All stages involved in the life cycle of a product from manufacturing till it reaches consumer are stored on the blockchain. This system consists of two modules:

- Backend Module in the form of ASP.NET Core WebApp
- Frontend Module in the form of a Xamarin.Forms App

Backend module creates and manages Supply BlockChain and all types of operations on Supply BlockChain are performed in this Backend module. Backend Module is implemented in ASP.NET Core WebApp which exposes REST APIs to perform different operations on Supply BlockChain through Frontend Mobile App.

Frontend module consists of a Mobile App built in Xamarin.Forms which provides the interface to interact with the blockchain at backend in a user-friendly manner. This App consumes APIs exposed by Backend WebApp and performs various operations.

**Download Android Application:** [SupplyBlockChain.apk](https://www.sudeshkumar.me/SupplyBlockChain.apk)


## **FrontEnd Module**

The Xamarin.Forms App aka FrontEnd Module provides interface to perform operations on SupplyChain depending on Access Level of users. It can't directly access the Supply BlockChain but makes API calls to the Backend WebApp and then Backend WebApp performs actions on Supply BlockChain. Depending on Access levels of a user, App can perform following tasks:

- Generate a unique ID of product in the form of QrCode to identify this product in Supply BlockChain when the product is created for the first time.
- Scan the QrCode and get product's Unique ID whenever a transaction is created about this product.
- View all the transactions performed on a product till now by scanning the QrCode.
- At the time of transaction creation, provide the GPS locations of mobile device and attach them with transaction.
- Provide interface to Create/Modify/Login user account.


## **BackEnd Module**

The ASP.NET WebApp aka Backend creates and manages the Supply BlockChain. This WebApp also contains one Block Miner and Three Block Verifiers. Every 40 minutes, a new block is mined by the Miner and verified by the three Verifiers. Following are the operations that can be performed by BackEnd Module:

- Create product transactions and push them to the pending transaction queue for mining.
- Mine pending transactions every 40 minutes.
- After mining, get block verifed by verifiers and if approved add it to the Supply BlockChain.
- Along with Main Supply BlockChain, maintain blockChains of verifiers also.
- Create/Modify User accounts with specified access levels.
- Provide Supply BlockChain to user whenever asked for.
- Provide interface for user to print QrCode of all products he/she created.

**Visit Backend Website:** <http://supplyblockchain.sudeshkumar.me>

**View BackEnd Source Code:** <https://github.com/sudesh1611/SupplyBlockChain_Backend>


## **Working Details**

**Supply BlockChain specifications**

- A new block is mined every 40 minutes.
- For Proof of Work,at present, miner have to mine block such that Hash of that block starts with 5 zeros.
- The difficulty of mining (i.e number of starting zeros of a block Hash) block can be increased by admin, if required.
- At present there is one Block Miner and Three Verifiers.
- All verifiers have their own independent Supply BlockChains and are identical to Main Supply BlockChain.
- At the time of transaction creation, app also records the location at which transaction is made.
- There are Four Access Levels.
    - **Admin:** Create/Modify User Account, Change difficulty of block mining, Create transaction, View a product's Transactions by scanning QrCode.
    - **Create Transaction:** Create Transaction, View a product's Transaction by scanning QrCode.
    - **Create Account:** Create/Modify User Account, View a product's Transactions by scanning QrCode.
    - When no access level is specified, user can only view a product's Transactions by scanning QrCode.

**Create New Product's Transaction**

New Product's transaction means that the product hasn't been given a unique ID. If a user have Create Transaction Right then only user can create transaction.

- Enter the product details in Mobile App by going to Create Transaction -> New Product.
- After entering details push "Create Product" button, it will generate a unique ID for product in the form of a QrCode.
- Take print of this QrCode either from mobile screenshot or from Website and attach it with product.
- Next time, whenever a new transaction is made on this product, QrCode is to be scanned.

**Create Old Product's Transaction**

Old Product's transaction means that the product already has been given a unique ID. If a user have Create Transaction access then only user can create transaction.

- Scan product's QrCode by going to Create Transaction -> Scan QrCode.
- Fill the information regarding the process done on product and push the "Create Transaction" button.
- If the transaction is commited successfuly, a success message will be displayed. This transaction will be visible on Supply BlockChain only after next mining process occurs.

**View Product's Transaction**

You can view all the transactions performed on a product by going to Scan QrCode on main screen.

**Create Account**

A user who have Right to create account can create account by going to Create Account and filling all details related to user being created. At the time of creation access rights to user are given.

**View BlockChain**

Admin can view Supply BlockChain in raw form by going to View BlockChain.


## **Links**

**Download Android Application:** [SupplyBlockChain.apk](https://www.sudeshkumar.me/SupplyBlockChain.apk)

*Note\*: When for the first time, you give Android Application access to camera, scanner might not open. If this happens, close application and remove it from background also.Then, start application again.*

**View my other projects:** <https://github.com/sudesh1611>

**Contact at** [sudesh1611@gmail.com](mailto:sudesh1611@gmail.com)
