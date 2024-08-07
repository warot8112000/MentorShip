## Table of Contents
- [Commonly used functions](#commonly-used-functions)
- [Question 1 - Delete an element from an array at a specified position](#question-1---delete-an-element-from-an-array-at-a-specified-position)
- [Question 2 - Find the element most frequently occurring in the array](#question-2---find-the-element-most-frequently-occurring-in-the-array)
- [Question 3 - Insert an element at a specific position in an array](#question-3---insert-an-element-at-a-specific-position-in-an-array)

# Question & Solution

## Commonly used functions
Check n:

<img width="433" alt="Initial Image" src="https://github.com/user-attachments/assets/6447c94b-16ab-46ad-884e-c7e626e661db">

Check position:

<img width="433" alt="Check position" src="https://github.com/user-attachments/assets/13fa82b8-0c59-4d73-aa6a-599d73258a89">

Random Array:

<img width="433" alt="Random Array" src="https://github.com/user-attachments/assets/8052fca5-fc99-438f-87dc-761cb4a063e3">

Print array:

<img width="433" alt="Print array" src="https://github.com/user-attachments/assets/7513f28a-2f23-464d-9010-c9a4faac8054">

### 1. Delete an element from an array at a specified position
Generate a list of arrays containing n elements with random integers ranging from 0 to 9.  Delete an element from an array at a specified position.

Main:

<img width="433" alt="Image 1" src="https://github.com/user-attachments/assets/192149f0-5794-4578-b86f-c428674132af">

The DeleteElement function removes an element from an array at a specified position. It does this by shifting all elements after the specified position one spot to the left. 

The function uses a loop to go through the array, and once it reaches the position to delete, it starts copying the next element into the current position. 
This effectively "removes" the element by overwriting it.

<img width="433" alt="Image 2" src="https://github.com/user-attachments/assets/399c21d7-8d96-4f7f-a6a3-91cd81b338e2">

### 2. Find the element most frequently in the array
Generate a list of arrays containing n elements with random integers ranging from 0 to 9. Find the element most frequently in the array.

Main:

<img width="433" alt="Main Image" src="https://github.com/user-attachments/assets/6ca24408-df8b-4423-a579-a0d49eaeefc9">

Find the element most frequently: Use an auxiliary array b to count how often each number (0 through 9) appears in array a.
Then determine the maximum frequency.

<img width="433" alt="Find Element Most Frequently" src="https://github.com/user-attachments/assets/b2103131-b62a-4d83-a458-6f52db540f49">

Print the element most frequently: Prints each number that has the highest frequency, ensuring that each number is printed only once

<img width="433" alt="Print Element Most Frequently" src="https://github.com/user-attachments/assets/522c275f-b34c-480d-8254-ec39157b574b">


### 3. Insert an element at a specific position in an array
Generate a list of arrays containing n elements with random integers ranging from 0 to 9. Insert an element at a specific position in an array

Main: 

<img width="433" alt="Main Image" src="https://github.com/user-attachments/assets/a80c9fa0-2331-443a-8e4a-7e3e5237bdaf">

Insert an element: Shift the elements after the specified position to make space for the new element.
And place the new element into the specified position

<img width="433" alt="Insert Element" src="https://github.com/user-attachments/assets/a01dfd6d-de9e-4a7e-be71-381be8a6d5c6">

