Test project for Professional Development Plan (PDP)

Project works according to this logic: in file "input.txt" entered transaction data, and the program processes it and transfer it into database "output". If some part of transaction is missed then database gets null. 

Program can create database file, however file "input.txt" is necessary.

"input.txt" contains data in this format:

item: [itemName]

quantity: [quantity]

price: [unitPrice]

The left part contains the description from the file, the right part contains the value for the program.
=======
Project works according to this logic: in file "input.json" entered transaction data, and the program processes it and transfer it into database "output". 

Program can create database file, however file "input.json" is necessary.

"input.json" contains data in this format (Json example):

[
  {
    "SellerId": "994",
    "Change": 10.0,
    "Items": [
      {
        "ItemName": "juice",
        "ItemCode": "J1",
        "Quantity": 3,
        "UnitPrice": 40.0
      },
      {
        "ItemName": "apple",
        "ItemCode": "A1",
        "Quantity": 1,
        "UnitPrice": 30.0
      }
    ]
  }
]

Fixed specified problems, TotalPrice calculated by program, added back Domain, added ParseLogic.
