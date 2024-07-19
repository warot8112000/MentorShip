--2 List All Clothing Items
--Display the name of clothing items (name the column clothes), their color (name the column color), and the last name and first name of the customer(s) who bought this apparel in 
--their favorite color. Sort rows according to color, in ascending order

SELECT CLO.name, CLO.color_id, CUS.first_name, CUS.last_name
FROM clothing AS CLO 
JOIN clothing_order as CO ON CO.clothing_id = CLO.id
JOIN customer AS CUS ON CUS.id = CO.customer_id
WHERE CLO.color_id = CUS.favorite_color_id
ORDER BY CLO.color_id

--3. Get All Non-Buying Customers
--Select the last name and first name of customers and the name of their favorite color for customers with no purchases.
SELECT first_name, last_name, favorite_color_id
FROM customer
WHERE id NOT IN (
SELECT customer_id
FROM clothing_order
GROUP BY customer_id
) -- cach nay khong toi uu

SELECT CUS.last_name, CUS.first_name, CUS.favorite_color_id
FROM customer AS CUS
LEFT JOIN clothing_order AS CO ON CUS.id = CO.customer_id
WHERE CO.clothing_id IS NULL
GROUP BY  CUS.last_name, CUS.first_name, CUS.favorite_color_id

--4. Select All Main Categories and Their Subcategories
--Select the name of the main categories (which have a NULL in the parent_id column) 
--and the name of their direct subcategory (if one exists). Name the first column category 
--and the second column subcategory.

SELECT CATE1.name AS MAIN, CATE2.name AS SUB
FROM category AS CATE1
JOIN category AS CATE2 ON CATE1.id = CATE2.parent_id
WHERE CATE1.parent_id IS NULL



CREATE TABLE color (
	id INT PRIMARY KEY,
	name varchar(50) NOT NULL,
	extra_fee DECIMAL(10,2)
);
CREATE TABLE customer (
	id INT PRIMARY KEY,
	first_name varchar(100),
	last_name varchar(100),
	favorite_color_id INT, 
	FOREIGN KEY (favorite_color_id) 
	REFERENCES color(id)
);

CREATE TABLE category (
	id INT PRIMARY KEY,
	name VARCHAR(100),
	parent_id int
);

CREATE TABLE clothing (
	id INT PRIMARY KEY,
	name varchar(100),
	size varchar(10) CHECK (size IN ('S', 'M', 'L', 'XL', '2XL', '3XL')),
	price DECIMAL(10,2),
	color_id INT, 
	FOREIGN KEY (color_id) 
	REFERENCES color(id),
	category_id int,
	FOREIGN KEY(category_id)
	REFERENCES category(id)
)

CREATE TABLE clothing_order (
	id INT PRIMARY KEY,
	customer_id int,
	clothing_id int,
	items_stores int,
	order_date date,
	FOREIGN KEY (customer_id) 
	REFERENCES customer(id),
	FOREIGN KEY(clothing_id)
	REFERENCES clothing(id)
);

INSERT INTO color (id, name, extra_fee) VALUES (100, 'Red', 5.00);
INSERT INTO color (id, name, extra_fee) VALUES (101, 'Blue', 3.00);
INSERT INTO color (id, name, extra_fee) VALUES (102, 'Green', 4.00);
INSERT INTO color (id, name, extra_fee) VALUES (103, 'Black', 2.00);
INSERT INTO color (id, name, extra_fee) VALUES (104, 'White', 1.00);

INSERT INTO category (id, name, parent_id) VALUES (1, 'Men', NULL);
INSERT INTO category (id, name, parent_id) VALUES (2, 'Women', NULL);
INSERT INTO category (id, name, parent_id) VALUES (3, 'Shirts', 1);
INSERT INTO category (id, name, parent_id) VALUES (4, 'Pants', 1);
INSERT INTO category (id, name, parent_id) VALUES (5, 'Dresses', 2);
INSERT INTO category (id, name, parent_id) VALUES (6, 'Skirts', 2);

INSERT INTO customer (id, first_name, last_name, favorite_color_id) VALUES (1, 'John', 'Doe', 100);
INSERT INTO customer (id, first_name, last_name, favorite_color_id) VALUES (2, 'Jane', 'Smith', 101);
INSERT INTO customer (id, first_name, last_name, favorite_color_id) VALUES (3, 'Alice', 'Johnson', 102);
INSERT INTO customer (id, first_name, last_name, favorite_color_id) VALUES (4, 'Bob', 'Brown', 103);
INSERT INTO customer (id, first_name, last_name, favorite_color_id) VALUES (5, 'Charlie', 'Davis', 104);
INSERT INTO customer (id, first_name, last_name, favorite_color_id) VALUES (6, 'THANG', 'TRAN', 100);

INSERT INTO clothing (id, name, size, price, color_id, category_id) VALUES (1, 'Men Shirt', 'M', 25.00, 100, 3);
INSERT INTO clothing (id, name, size, price, color_id, category_id) VALUES (2, 'Men Pants', 'L', 30.00, 101, 4);
INSERT INTO clothing (id, name, size, price, color_id, category_id) VALUES (3, 'Women Dress', 'S', 50.00, 102, 5);
INSERT INTO clothing (id, name, size, price, color_id, category_id) VALUES (4, 'Women Skirt', 'M', 20.00, 103, 6);
INSERT INTO clothing (id, name, size, price, color_id, category_id) VALUES (5, 'Men Shirt', 'XL', 27.50, 104, 3);

INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (1, 1, 1, 10, '2024-07-01');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (2, 2, 2, 5, '2024-07-02');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (3, 3, 3, 7, '2024-07-03');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (4, 4, 4, 3, '2024-07-04');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (5, 5, 5, 8, '2024-07-05');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (6, 1, 2, 6, '2024-07-06');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (7, 2, 3, 4, '2024-07-07');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (8, 3, 4, 2, '2024-07-08');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (9, 4, 5, 1, '2024-07-09');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (10, 5, 1, 9, '2024-07-10');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (11, 1, 3, 7, '2024-07-11');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (12, 2, 4, 5, '2024-07-12');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (13, 3, 5, 8, '2024-07-13');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (14, 4, 1, 4, '2024-07-14');
INSERT INTO clothing_order (id, customer_id, clothing_id, items_stores, order_date) VALUES (15, 5, 2, 3, '2024-07-15');
