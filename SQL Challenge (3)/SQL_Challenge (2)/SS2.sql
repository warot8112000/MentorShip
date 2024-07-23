/*
The runner table contains the following columns:
	id stores the unique ID of the runner.
	name stores the runner's name.
	main_distance stores the distance (in meters) that the runner runs during events.
	age stores the runner's age.
	is_female indicates if the runner is male or female.

The event table contains the following columns:
	id stores the unique ID of the event.
	name stores the name of the event (e.g. London Marathon, Warsaw Runs, or New Year Run).
	start_date stores the date of the event.
	city stores the city where the event takes place.
	
The runner_event table contains the following columns:
	runner_id stores the ID of the runner.
	event_id stores the ID of the event.
*/

CREATE TABLE runner_event (
    runner_id INT,
    event_id INT,
    PRIMARY KEY (runner_id, event_id),
    FOREIGN KEY (runner_id) REFERENCES runner(runner_id),
    FOREIGN KEY (event_id) REFERENCES event(event_id)
);

CREATE TABLE event (
    event_id INT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    start_date DATE NOT NULL,
    city VARCHAR(255) NOT NULL
);


CREATE TABLE runner (
	runner_id INT PRIMARY KEY,
	name VARCHAR(100),
	main_distance INT,
	age INT,
	is_female BIT
)

--2. Organize Runners Into Groups
--Select the main distance and the number of runners that run the given distance (runners_number). 
--Display only those rows where the number of runners is greater than 3.
SELECT COUNT(*)
FROM runner
WHERE main_distance > 3
GROUP BY runner_id, main_distance

SELECT *
FROM runner_event

SELECT *
FROM event

/*
3.How Many Runners Participate in Each Event
Display the event name and the number of club members that take part in this event (call this column runner_count). 
Note that there may be events in which no club 
members participate. For these events, the runner_count should equal 0. */
SELECT E.name, COUNT(*) AS RUNNER_COUNT
FROM event AS E
LEFT JOIN runner_event AS RE ON E.event_id = RE.event_id
GROUP BY RE.event_id, E.name
/*
4.Group Runners by Main Distance and Age
Display the distance and the number of runners there are for the following age categories: under 20, 20–29, 30–39, 40–49, and over 50. 
Use the following column aliases: under_20, age_20_29, age_30_39, age_40_49, and over_50.*/
SELECT *
FROM runner

SELECT main_distance, 
SUM(CASE WHEN age < 20 THEN 1 ELSE 0 END) AS under_20,
SUM(CASE WHEN age >= 20 AND age <= 29 THEN 1 ELSE 0 END) AS F20_29,
SUM(CASE WHEN age >= 30 AND age <= 39 THEN 1 ELSE 0 END) AS F30_39,
SUM(CASE WHEN age >= 40 AND age <= 49 THEN 1 ELSE 0 END) AS F40_49,
SUM(CASE WHEN age > 50 THEN 1 ELSE 0 END) AS OVER50
FROM runner
GROUP BY main_distance	