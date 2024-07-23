### Question 1: How many customers has Foodie-Fi ever had?

```SQL
SELECT COUNT(DISTINCT customer_id) AS NUMBER_CUSTOMERS
FROM foodie_fi.subscriptions;
```

### Question 2: What is the monthly distribution of trial plan `start_date` values for our dataset? (i.e., the monthly count of users on the trial plan subscription.)

```SQL
SELECT MONTH(start_date), COUNT(customer_id)
FROM foodie_fi.subscriptions
WHERE plan_id = 0
GROUP BY MONTH(start_date)
ORDER BY MONTH(start_date);
```

### Question 3: What plan `start_date` values occur after the year 2020 for our dataset? Show the breakdown by count of events for each `plan_name`.

```SQL
SELECT P.plan_name, P.plan_id, COUNT(S.customer_id)
FROM foodie_fi.plans AS P
JOIN foodie_fi.subscriptions AS S ON P.plan_id = S.plan_id
WHERE S.start_date >= '2021-01-01'
GROUP BY P.plan_name, P.plan_id;
```

### Question 4: What is the customer count and percentage of customers who have churned rounded to 1 decimal place?

```SQL
SELECT COUNT(DISTINCT customer_id) AS CHURNED_TOTAL, 
ROUND(100.0 * COUNT(DISTINCT customer_id) / (SELECT COUNT(DISTINCT customer_id) FROM foodie_fi.subscriptions), 1) AS CHURNED_PERCENT
FROM foodie_fi.subscriptions
WHERE plan_id = 4;
```

### Question 5: How many customers have churned straight after their initial free trial? What percentage is this rounded to the nearest whole number?

```SQL
WITH ranked_startdate_cte AS (
  SELECT S.customer_id, S.plan_id, 
  ROW_NUMBER() OVER (PARTITION BY S.customer_id ORDER BY S.start_date) AS row_num
  FROM foodie_fi.subscriptions AS S
)
SELECT COUNT(*) AS NUM, 
ROUND(COUNT(*) * 100 / (SELECT COUNT(DISTINCT customer_id) FROM foodie_fi.subscriptions), 1) AS PER
FROM ranked_startdate_cte
WHERE row_num = 2 AND plan_id = 4;
```

### Question 6: What is the number and percentage of customer plans after their initial free trial?

```SQL
WITH NEXT_PLAN_CTE AS (
  SELECT SUB.customer_id, P.plan_name, P.plan_id,
  LEAD(P.plan_name) OVER (PARTITION BY SUB.customer_id ORDER BY SUB.start_date) AS NEXT_PLAN
  FROM foodie_fi.subscriptions AS SUB
  JOIN foodie_fi.plans AS P ON SUB.plan_id = P.plan_id
)
SELECT NEXT_PLAN, COUNT(customer_id) AS NUM_CUSTOMERS,
ROUND((COUNT(customer_id) * 100.0 / (SELECT COUNT(DISTINCT customer_id) FROM foodie_fi.subscriptions)), 1) AS PER
FROM NEXT_PLAN_CTE
WHERE NEXT_PLAN IS NOT NULL AND plan_id = 0
GROUP BY NEXT_PLAN;
```

### Question 7: What is the customer count and percentage breakdown of all 5 `plan_name` values at 2020-12-31?

```SQL
WITH next_dates AS (
  SELECT
    customer_id,
    plan_id,
    start_date,
    LEAD(start_date) OVER (PARTITION BY customer_id ORDER BY start_date) AS next_date
  FROM foodie_fi.subscriptions
  WHERE start_date <= '2020-12-31'
)
SELECT
  plan_id, 
  COUNT(DISTINCT customer_id) AS customers,
  ROUND(100.0 * COUNT(DISTINCT customer_id) / (SELECT COUNT(DISTINCT customer_id) FROM foodie_fi.subscriptions), 1) AS percentage
FROM next_dates
WHERE next_date IS NULL
GROUP BY plan_id;
```

### Question 8: How many customers upgraded to an annual plan in 2020?

```SQL
SELECT COUNT(DISTINCT customer_id)
FROM foodie_fi.subscriptions
WHERE plan_id = 3 AND start_date <= '2020-12-31';
```

### Question 9: On average, how many days does it take for a customer to upgrade to an annual plan from the day they join Foodie-Fi?

```SQL
WITH DAY_JOIN_CTE AS (
  SELECT customer_id, start_date AS join_date
  FROM foodie_fi.subscriptions
  WHERE plan_id = 0
)
SELECT AVG(DATEDIFF(day, DYCTE.join_date, S.start_date))
FROM foodie_fi.subscriptions AS S 
JOIN DAY_JOIN_CTE AS DYCTE ON DYCTE.customer_id = S.customer_id
WHERE plan_id = 3;
```

### Question 10: Can you further break down this average value into 30-day periods (i.e., 0-30 days, 31-60 days, etc.)?

```SQL
WITH trial_plan AS (
    SELECT 
        customer_id, 
        start_date AS trial_date
    FROM foodie_fi.subscriptions
    WHERE plan_id = 0
), 
annual_plan AS (
    SELECT 
        customer_id, 
        start_date AS annual_date
    FROM foodie_fi.subscriptions
    WHERE plan_id = 3
), 
bins AS (
    SELECT 
        trial.customer_id,
        DATEDIFF(day, trial.trial_date, annual.annual_date) AS days_to_upgrade,
        CASE 
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 0 AND 30 THEN 1
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 31 AND 60 THEN 2
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 61 AND 90 THEN 3
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 91 AND 120 THEN 4
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 121 AND 150 THEN 5
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 151 AND 180 THEN 6
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 181 AND 210 THEN 7
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 211 AND 240 THEN 8
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 241 AND 270 THEN 9
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 271 AND 300 THEN 10
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 301 AND 330 THEN 11
            WHEN DATEDIFF(day, trial.trial_date, annual.annual_date) BETWEEN 331 AND 365 THEN 12
            ELSE NULL
        END AS avg_days_to_upgrade
    FROM trial_plan AS trial
    JOIN annual_plan AS annual ON trial.customer_id = annual.customer_id
)
SELECT 
    CONCAT((avg_days_to_upgrade - 1) * 30, ' - ', avg_days_to_upgrade * 30, ' days') AS bucket, 
    COUNT(*) AS num_of_customers
FROM bins
GROUP BY avg_days_to_upgrade
ORDER BY avg_days_to_upgrade;
```

