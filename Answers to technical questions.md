### 1. How long did you spend on the coding assignment? What would you add to your solution if you had more time? If you didn't spend much time on the coding assignment then use this as an opportunity to explain what you would add.

I spent around 4 hours total on this assignment. I think it was an adequate time and if I would add something to it I would probably add a better UI and API Caching


### 2. What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it.

One of my favorite features added recently to C# is the collection initializer. Previously if we had to initialize an array, we had to do `string[] currencyExchanges = new string[] { "USD" };` 
Now we can just use the square brackets to initialize arrays and ANY collection type like Lists making code a lot easier to read. Below is a snippet from the test classes
```
        // Arrange
        string symbol = "";
        string[] currencyExchanges = ["USD"];
```


### 3. How would you track down a performance issue in production? Have you ever had to do this?

Taking a deeper look into any long running operations or non-performant code is how I would first identify the situation. In production, we need to have tracing on our calls in order to no which section of our codebase is taking the longest to complete
Example that happened with me: Our company had a collaboration with a famous influencer. The moment the influencer went live, a lot of users rushed into our site and tried creating accounts. However, our cryptography code that is used to generate and verify passwords was not so optimap,
so our site was going down a lot. We had to increase the cluster size by 4 times to handle the increased traffic and after that we tackled the slow code separately 


### 4. What was the latest technical book you have read or tech conference you have been to? What did you learn?

I watched the newest .NET conference where they revealed the new .NET 9 version. It was a pleasent experience and I learned a lot about Open Telemetry


### 5. What do you think about this technical assessment?

It was a good assessment to measure the capabilities of a Senior Developer. This is much better than leet code questions that developers can memorize. 
This assessment was also fun because it is similar to a small hackathon where we have a small time to build something cool


### 6. Please, describe yourself using JSON.

```
{
  "name": "Makram",
  "title": "Full Stack Engineer",
  "experience": {
    "years": 10,
    "roles": ["Senior Full Stack Engieer", "Consultant"],
    "highlights": [
      "Designed and maintained scalable solutions",
      "Lead a small team of developers in dilvering client requirements",
      "Continue to learn and grow my skills through fun side projects and experiments"
    ]
  },
  "skills": {
    "primary": ["C#", ".NET", "React", "Python", "NoSQL", "SQL Databases"],
    "secondary": ["Generative AI",  "Software Architecture", "Agile"],
    "fun": ["Building AI Agents"]
  },
  "personality": {
    "traits": ["Curious", "Innovative", "Collaborative"],
    "quirks": "Loves blending tech with creativity."
  },
  "hobbies": ["Game Development", "Gaming", "Movies", "Gym"],
  "career_goals": "To inspire and mentor teams in building innovative solutions, inspiring a culture of collaboration, continuous learning, and technical excellence.",
  "favorite_quote": "Code is like humor. When you have to explain it, it’s bad."
}

```
