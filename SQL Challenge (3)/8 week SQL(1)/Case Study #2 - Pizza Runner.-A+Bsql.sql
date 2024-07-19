-- A. Pizza Metrics

-- 1. How many pizzas were ordered?
SELECT COUNT(*)
FROM pizza_runner.customer_orders
-- 2. How many unique customer orders were made?
SELECT COUNT(DISTINCT order_id) AS unique_order_count
FROM pizza_runner.customer_orders;
-- 3. How many successful orders were delivered by each runner?
SELECT runner_id, COUNT(order_id)
FROM pizza_runner.runner_orders
WHERE pickup_time != 'NULL'
GROUP BY runner_id
-- 4. How many of each type of pizza was delivered?
SELECT PN.pizza_name, COUNT(RO.order_id)
FROM pizza_runner.pizza_names AS PN
JOIN pizza_runner.customer_orders AS CO ON PN.pizza_id = CO.pizza_id
JOIN pizza_runner.runner_orders AS RO ON CO.order_id = RO.order_id
WHERE RO.duration != '0'
GROUP BY PN.pizza_name
SELECT 
  p.pizza_name, 
  COUNT(c.pizza_id) AS delivered_pizza_count
FROM pizza_runner.customer_orders AS c
JOIN pizza_runner.runner_orders AS r
  ON c.order_id = r.order_id
JOIN pizza_runner.pizza_names AS p
  ON c.pizza_id = p.pizza_id
WHERE r.distance != '0'
GROUP BY p.pizza_name;

-- 5. How many Vegetarian and Meatlovers were ordered by each customer?
SELECT CO.customer_id, COUNT(CASE WHEN PN.pizza_name = 'Meatlovers' THEN 1 END) AS MeatloversPIZZA,
 COUNT(CASE WHEN PN.pizza_name = 'Vegetarian' THEN 1 END) AS VegetarianPIZZA
FROM pizza_runner.pizza_names AS PN
JOIN pizza_runner.customer_orders AS CO ON PN.pizza_id = CO.pizza_id
GROUP BY CO.customer_id
-- 6. What was the maximum number of pizzas delivered in a single order?
SELECT TOP 1 PERCENT CO.order_id, COUNT(pizza_id)
FROM pizza_runner.customer_orders AS CO 
JOIN pizza_runner.runner_orders RO ON CO.order_id = RO.order_id
WHERE RO.distance != '0'
GROUP BY CO.order_id
ORDER BY COUNT(pizza_id) DESC


-- 7. For each customer, how many delivered pizzas had at least 1 change and how many had no changes?
SELECT CO.customer_id, 
SUM(CASE WHEN CO.exclusions <> ' ' OR CO.extras <> ' ' THEN 1 ELSE 0 END) AS AT_LEAST_1_CHANGE,
SUM(CASE WHEN CO.exclusions = ' ' AND CO.extras = ' ' THEN 1 ELSE 0 END ) AS NO_CHANGE
FROM pizza_runner.customer_orders as CO
JOIN pizza_runner.runner_orders AS RO ON CO.order_id = RO.order_id
WHERE RO.distance != '0'
GROUP BY CO.customer_id

-- 8. How many pizzas were delivered that had both exclusions and extras?COUNT(CO.pizza_id)
SELECT COUNT(CO.pizza_id)
FROM pizza_runner.pizza_names AS PN
JOIN pizza_runner.customer_orders AS CO ON PN.pizza_id = CO.pizza_id
JOIN pizza_runner.runner_orders AS RO ON CO.order_id = RO.order_id
WHERE RO.duration != '0' AND CO.exclusions <> ' ' AND CO.extras <> ' '


-- 9. What was the total volume of pizzas ordered for each hour of the day?
SELECT DATEPART(HOUR, order_time) AS order_hour, COUNT(pizza_id)
FROM pizza_runner.customer_orders
GROUP BY DATEPART(HOUR, order_time) 

SELECT DATEPART(HOUR, order_time) AS order_hour,  DATEPART(DAY, order_time) AS order_day, COUNT(pizza_id)
FROM pizza_runner.customer_orders
GROUP BY DATEPART(HOUR, order_time),  DATEPART(DAY, order_time)
-- 10. What was the volume of orders for each day of the week?
SELECT FORMAT(DATEADD(DAY,2, order_time),'dddd') AS order_weekday, COUNT(pizza_id)
FROM pizza_runner.customer_orders
GROUP BY FORMAT(DATEADD(DAY,2, order_time),'dddd')
-- Data Cleaning & Transformation
--CUSTOMER_ORDERS
SELECT 
    order_id,
    customer_id,
    pizza_id,
    CASE
        WHEN exclusions IS NULL OR exclusions = 'null' THEN ' '
        ELSE exclusions
    END AS exclusions,
    CASE
        WHEN extras IS NULL OR extras = 'null' THEN ' '
        ELSE extras
    END AS extras,
    order_time
INTO #customer_orders_temp
FROM pizza_runner.customer_orders;

-- RUNNER_ORDERS
SELECT 
  order_id, 
  runner_id,  
  CASE
	  WHEN pickup_time LIKE 'null' THEN ' '
	  ELSE pickup_time
	  END AS pickup_time,
  CASE
	  WHEN distance LIKE 'null' THEN ' '
	  WHEN distance LIKE '%km' THEN TRIM('km' from distance)
	  ELSE distance 
    END AS distance,
  CASE
	  WHEN duration LIKE 'null' THEN ' '
	  WHEN duration LIKE '%mins' THEN TRIM('mins' from duration)
	  WHEN duration LIKE '%minute' THEN TRIM('minute' from duration)
	  WHEN duration LIKE '%minutes' THEN TRIM('minutes' from duration)
	  ELSE duration
	  END AS duration,
  CASE
	  WHEN cancellation IS NULL or cancellation LIKE 'null' THEN ' '
	  ELSE cancellation
	  END AS cancellation
INTO #runner_orders_temp
FROM pizza_runner.runner_orders;

/*
ALTER COLUMN pickup_time DATETIME;
ALTER COLUMN duration INT;
ALTER TABLE #runner_orders_temp
ALTER COLUMN duration INT;*/

SELECT * FROM pizza_runner.pizza_names
SELECT * FROM pizza_runner.customer_orders
SELECT * FROM pizza_runner.runner_orders
SELECT * FROM pizza_runner.runners

-- B. Runner and Customer Experience  --- CAU 1?

-- 1. How many runners signed up for each 1 week period? (i.e. week starts 2021-01-01)
SELECT DATEPART(WEEK, registration_date) AS WEEK_PERIOD, COUNT(runner_id)
FROM pizza_runner.runners
GROUP BY DATEPART(WEEK, registration_date);

-- 2. What was the average time in minutes it took for each runner to arrive at the Pizza Runner HQ to pickup the order?
WITH time_taken_cte as (
SELECT RO.order_id, CO.order_time, RO.pickup_time, DATEDIFF(MINUTE, CO.order_time, RO.pickup_time) AS pickup_minutes
FROM pizza_runner.customer_orders AS CO
JOIN pizza_runner.runner_orders AS RO ON CO.order_id = RO.order_id
WHERE RO.distance != '0'
GROUP BY RO.order_id, CO.order_time, RO.pickup_time
)
SELECT AVG(pickup_minutes)
FROM time_taken_cte;
-- 3. Is there any relationship between the number of pizzas and how long the order takes to prepare?
WITH PREPARE_TIME_PIZZA AS (
SELECT CO.order_id, CO.order_time, RO.pickup_time, COUNT(CO.order_id) AS NUMBER_PIZZA,
DATEDIFF(MINUTE, CO.order_time, RO.pickup_time) AS PREPARE_TIME
FROM pizza_runner.customer_orders AS CO
JOIN pizza_runner.runner_orders AS RO ON CO.order_id = RO.order_id
WHERE RO.distance != '0'
GROUP BY CO.order_id, CO.order_time, RO.pickup_time
)

SELECT NUMBER_PIZZA, AVG(PREPARE_TIME) AS AVG_PREPARE_TIME
FROM PREPARE_TIME_PIZZA
GROUP BY NUMBER_PIZZA

-- 4. What was the average distance travelled for each customer?
SELECT   c.customer_id,  AVG(r.distance) AS avg_distance
FROM PIZZA_RUNNER.customer_orders AS c
JOIN #runner_orders_temp AS r
  ON c.order_id = r.order_id
  WHERE r.distance != '0'
GROUP BY c.customer_id;
-- 5. What was the difference between the longest and shortest delivery times for all orders?
SELECT *
FROM #runner_orders_temp

SELECT MAX(CAST(duration AS FLOAT)) - MIN(CAST(duration AS FLOAT))  AS delivery_time_difference
FROM #runner_orders_temp
where duration IS NOT NULL;

-- 6. What was the average speed for each runner for each delivery and do you notice any trend for these values?
SELECT runner_id, order_id, distance, duration, 
       (CASE WHEN duration > 0 THEN ROUND(distance / (duration / 60.0), 2) ELSE NULL END) AS speed_kmph
FROM #runner_orders_temp
WHERE distance != 0;


-- 7. What is the successful delivery percentage for each runner?
SELECT runner_id, (SUM(CASE WHEN distance IS NOT NULL THEN 1 ELSE 0 END) * 100)/COUNT(*) AS SS_DELI_PERC
FROM #runner_orders_temp
GROUP BY runner_id


