# Project Name - Tent House Rental

- Backened is done via .NET Core Web Api
- Frontend html,css, javascript, bootstrap
- In this Project, an authorize User can rent out/in the products available.
- Add new products, customer, transactions

 ### Product Interface -
- Display all products with available quantity and quantity booked .
- Product price, quantity can be updated .
- New product can be added .

### Customer Interface -
- Display all customers.
- New customer can be added.

### Transaction Interface - 
- All transaction of products .
- New transaction can be made (IN/OUT).
- Existing transaction can be cleared.

### Reports -
#### Inventory Summary Report
```
Item ID   Item Name                   Available Quantity
1         Plastic Chairs              10000
2         Tiffany Chairs              5000
3         Bridal Chair                10
4         Plastic Round Tables        100
5         Plastic Rectangular Table   90
6         Steel Folding Table         80
7         Gas Stoves                  25
8         Chair Covers                6000
9         Table Cloths                500
```
#### Inventory Detailed Report :
filter by month/date ( by default all transaction is shown)
```
Item Name: Plastic Chairs
Available Quantity: 10000
---
Item Name: Tiffany Chairs
Available Quantity: 5000
---
Item Name: Bridal Chair
Available Quantity: 10

Transaction ID   Date/time             Type    Quantity
123456           2019-08-01 14:20:32   OUT     1
123460           2019-08-04 10:30:16   IN      1
---
Item Name: Plastic Round Tables
Available Quantity: 100
---
Item Name: Plastic Rectangular Table
Available Quantity: 90
---
Item Name: Steel Folding Table
Available Quantity: 80

Transaction ID   Date/time             Type    Quantity
123456           2019-08-01 14:20:30   OUT     15
123459           2019-08-04 10:30:15   IN      15
---
Item Name: Gas Stoves
Available Quantity: 23

Transaction ID   Date/time             Type    Quantity
123458           2019-08-01 14:20:34   OUT     2
---
Item Name: Plastic Rectangular Table
Available Quantity: 6000
---
Item Name: Plastic Rectangular Table
Available Quantity: 500
```



