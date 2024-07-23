## Entity Relationship Diagram
![127271130-dca9aedd-4ca9-4ed8-b6ec-1e1920dca4a8](https://github.com/user-attachments/assets/141d519f-5c4e-46dc-b9c1-e365240ce7dd)
## Question and Solution

### 1. What is the total amount each customer spent at the restaurant?
```SQL
SELECT 
    s.customer_id, 
    SUM(m.price)
FROM dannys_diner.menu AS m
JOIN dannys_diner.sales AS s ON m.product_id = s.product_id
GROUP BY s.customer_id;
```
### 2. How many days has each customer visited the restaurant?
```SQL
SELECT customer_id, COUNT(DISTINCT order_date)
FROM dannys_diner.sales
GROUP BY customer_id;
```
###  3. What was the first item from the menu purchased by each customer?
```SQL
WITH ordered_sales AS (
    SELECT S.customer_id, S.order_date, M.product_name,
    DENSE_RANK() OVER(
        PARTITION BY S.customer_id
        ORDER BY S.order_date
    ) AS ORDER_RANK
    FROM dannys_diner.sales AS S
    JOIN dannys_diner.menu AS M ON S.product_id = M.product_id
)
SELECT customer_id, product_name
FROM ordered_sales
WHERE ORDER_RANK = 1
GROUP BY customer_id, product_name;
```
### 4. What is the most purchased item on the menu and how many times was it purchased by all customers?
```SQL
WITH PurchasedItemByCus AS (
    SELECT customer_id, product_id, COUNT(product_id) AS CountProduct
    FROM dannys_diner.sales
    GROUP BY customer_id, product_id
    ORDER BY customer_id
)
SELECT product_id, SUM(CountProduct)
FROM PurchasedItemByCus
GROUP BY product_id
ORDER BY SUM(CountProduct) DESC
LIMIT 1;
```

### 5. Which item was the most popular for each customer?
```SQL
WITH MostPurchasedItemByCustomer AS (
    SELECT S.customer_id, S.product_id, M.product_name,
           COUNT(S.product_id) AS CountProduct,
           DENSE_RANK() OVER(
               PARTITION BY S.customer_id
               ORDER BY COUNT(S.product_id) DESC
           ) AS RANK1
    FROM dannys_diner.sales AS S
    JOIN dannys_diner.menu AS M ON S.product_id = M.product_id
    GROUP BY S.customer_id, S.product_id, M.product_name
    ORDER BY S.customer_id
)
SELECT customer_id, product_name, CountProduct
FROM MostPurchasedItemByCustomer
WHERE RANK1 = 1
ORDER BY customer_id;
```

### 6. Which item was purchased first by the customer after they became a member (Có gồm cả khách hàng chưa là mem hay không?)
```SQL
WITH PurchasedFirstByMem AS (
    SELECT M.product_name, MEM.customer_id, S.order_date,
           DENSE_RANK() OVER(
               PARTITION BY MEM.customer_id
               ORDER BY S.order_date
           ) AS RANK1
    FROM dannys_diner.menu AS M
    JOIN dannys_diner.sales AS S ON M.product_id = S.product_id
    JOIN dannys_diner.members AS MEM ON S.customer_id = MEM.customer_id
    WHERE MEM.join_date < S.order_date
)
SELECT *
FROM PurchasedFirstByMem
WHERE RANK1 = 1;
```
-- 7. Which item was purchased just before the customer became a member? (First?)
SELECT S.customer_id, M.product_name

