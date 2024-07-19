--1. How many unique nodes are there on the Data Bank system?
SELECT *
FROM customer_nodes
SELECT *
FROM customer_transactions
SELECT *
FROM regions

SELECT COUNT (DISTINCT node_id)
FROM customer_nodes
--2- What is the number of nodes per region?
SELECT RE.region_name, COUNT(CNODE.node_id) AS NODE_PER_REGION
FROM customer_nodes AS CNODE
JOIN regions AS RE ON CNODE.region_id = RE.region_id
GROUP BY RE.region_name
--3. How many customers are allocated to each region?
SELECT RE.region_name, COUNT(CNODE.customer_id)
FROM customer_nodes AS CNODE
JOIN regions AS RE ON CNODE.region_id = RE.region_id
GROUP BY RE.region_name
--4. How many days on average are customers reallocated to a different node?


WITH node_days AS (
  SELECT 
    customer_id, 
    node_id,
    DATEDIFF(DAY, start_date, end_date) AS days_in_node
  FROM customer_nodes
  WHERE end_date != '9999-12-31'
  GROUP BY customer_id, node_id, start_date, end_date
) 

  SELECT 
    customer_id,
    node_id,
    SUM(days_in_node) AS total_days_in_node
  FROM node_days
  GROUP BY customer_id, node_id
)

SELECT ROUND(AVG(total_days_in_node),0) AS avg_node_reallocation_days
FROM total_node_days;
--5. What is the median, 80th and 95th percentile for this same reallocation days metric for each region?