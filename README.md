# What is the Universal Hive Bot

This is a solution to my problems. I have a few tokesn which I need to stake, delegate stake, or sell. This processes are long and anoying so I need a small bot to do this taks for me.

Any question, advices or idea please create the issue.

# Why do I need to specyfy ther Active Key? 

Because bot is doing token operation it requires the ActiveKey to be able to authorise. I have added the posting key as to future-proof it just in case I will find an operation which can be automated and requires the posting authorization.

# But it can be better..

I am aware that there are a few places where I cna improve the bot. For example, I am sure if Balance and Flush action should be explicity declared or not. 

# List of available tasks

 1. Balance - get the lsit of available level 2 tokens
 ```
{
    "name": "Balance" 
}
```

 2. Flush - gathered all scheduled operations and passes them to HIVE Blockchain. That way the bot can control the maximum number of operations per block. 
 ```
{
    "name": "Flush"
}
 ```

  3. Transfer - allows to transfer tokens to another acount 

  Parameters:
    * token - the name of the token
    * amount - the computable desired amount, check #AmountCalculation to see what can be used
    * transferTo - account to which token ahas to be transfer 
```
{
    "name": "Transfer",
    "parameters": {
        "token": "BEE",
        "amount": "*",
        "transferTo": "alamut-he"
    }
}
```

 4. Stake - transfer tokesn to stake

 Parameters:
    * token - the name of the token
    * amount - the computable desired amount, check #AmountCalculation to see what can be used
```
{
    "name": "Stake",
    "parameters": {
    "token": "ONEUP",
    "amount": "*"
    }
}
```

 5. Unstake - transfer tokens out of stake 

 Parameters:
    * token - the name of the token
    * amount - the computable desired amount, check #AmountCalculation to see what can be used
```
{
    "name": "Unstake",
    "parameters": {
        "token": "BEE",
        "amount": "*"
}
```

 6. DelegateStake - delegates staked tokens 

 Parameters:
    * token - the name of the token
    * amount - the computable desired amount, check #AmountCalculation to see what can be used
    * delegateTo - accoutn to which tokens will be delegated 
```
{
    "name": "DelegateStake",
    "parameters": {
        "token": "PGM",
        "amount": "* - 50",
        "delegateTo": "lolz.pgm"
    }
}
```

 7. UndelegateStake - undelegates token 

 Parameters:
    * token - the name of the token
    * amount - the computable desired amount, check #AmountCalculation to see what can be used
    * undelegateFrom - account from which tokens will be undelegated 

```
{
    "name": "UndelegateStake",
    "parameters": {
    "token": "PGM",
    "amount": "* - 50",
    "undelegateFrom": "lolz.pgm"
    }
}
```

 8. Sell - opening the order on the market, the query is simple and is trying to find the fist amount larger than the requested one

 Parameters:
    * token - the name of the token
    * amount - the computable desired amount, check #AmountCalculation to see what can be used

```
{
    "name": "Sell",
    "parameters": {
    "token": "WOO",
    "amountToSell": "*"
    }
}
```

 9. Add Liqiditity to a pool - allows to add a liqidity to a provided pool. Action is calculating the amount for two scenarions, when left or right amount can be processed 

 Parameters:
    * tokenPair - a name of the liqidity pool express as pair of tokes eg. SPORTS:AFIT
    * leftAmount - the computable desired amount of left token, check #AmountCalculation to see what can be used 
    * rightAmount - the computable desired amount of right token, check #AmountCalculation to see what can be used 

```
{     
    "name": "AddToPool",
    "parameters": {
        "tokenPair": "PKM:SPS",
        "leftAmount": "*",
        "rightAmount": "*"
    }
}
```
 10. Swap tokens - allows to sent the token swap request to block chain 

 Parameters: 
    * tokenPair - a selection of tokens to convert following the pool syntach <left token>:<right token>
    * token - a token to be a input for swap
    * amountToSwap - the computable desired amount of source token 
    * maxSlippage - amaximun diffrence desired, for now it has to be provided but I will try to make it automatic if possible

```
{
    "name": "SwapToken",
    "parameters": {
        "tokenPair": "SPORTS:AFIT",
        "token": "SPORTS",
        "amountToSwap": "*",
        "maxSlippage": "0.673619"
    }
}
```

11. Terracore balance - gets teh value of avaible SCRAPS for claim

```
{
    "name": "terracore_balance"
}
```

12. Terracore SCRAP claim - allows to sent the token swap request to block chain 

 Parameters: 
    * amountToSwap - the computable desired amount which will be claimed

```
{
    "name": "terracore_claim",
    "parameters": {
        "amount": "*"
    }
}
```

13. ReadPost - used HIVE BridgeAPI to get top 20 ranked posts for given tag 

 Parameters: 
    * tag - a tag which post has to be tagged 
    * label - a varaible which will store the posts, this is configurable as to allow storing various posts at the same time

```
{
    "name": "read_posts",
    "parameters": {
        "tag": "youtrtag",
        "label": "posts"
    }
}
```

14. Variable - allows to specify a variable to be shared between other tassk. 

 Parameters: 


        {
          "name": "variable",
          "parameters": {
            "name": "weight",
            "value":"15.00"
          }
        },
        {
          "name": "load_template",
          "parameters": {
            "templateId": "Assassyn/9a3b5399b7439134870cc278f09fea38/raw/3d9b03614247c01e2fd16e8ca277bcafeff7a005",
            "label": "posts_template" //optional
          }
        },
        {
          "name": "vote_on_posts",
          "parameters": {
            "label": "posts",
            "useVaraible":  true
          }
        },
        {
          "name": "comment_on_posts",
          "parameters": {
            "label": "posts",
            "template": "posts_template"
          }
        },


14. load_template - returns a string value from GITHUB's GIST collection, Template is using a templating library to make it more powerfull. There are added two support function to access entity(the bot context object). **ReadEntity** used to read a single type entity and **EnumerateEntity** whixch allows to read a collection based properties(one retunred by ReadComments, ReadPosts actions). 

    Please see the documentation at https://github.com/craigbridges/Nettle/wiki for more details .

 Parameters: 
    * templateId - a part of ulr fomr ggithub's gist, e.g. <username>/12233423423423423423432423423433/raw/1234534545345345345abcdef1234566778890ab
    * label - local variable which will store the remplate value for later use

```
{
  "name": "load_template",
  "parameters": {
    "templateId": "<username>/12233423423423423423432423423433/raw/1234534545345345345abcdef1234566778890ab",
    "label": "example_template"
}
```

Example template: 

```
Daily Report for {{@GetDate()}}

# tl;dr

This is a daily summary of all post which has been supported by dcrop Boost 

# Voted on posts
{{ var posts = @EnumerateEntity("post") }}
{{each posts}}
  * {{$.name}}
{{/each}}
```

15. write_post - a action used to sent submit new post 

  Parameters: 
    * template - an name of the entity where template is loaded 
    * title - a tempalte for the title 
    * tags - a comma separated list of tags to use

```
{
    "name": "write_post",
    "parameters": {
        "template": "summary_post",
        "title": "dCropsboost daily upvoted posts collection - {{@FormatDate(@GetDate(), \"dd/MM/yyyy\")}}",
        "tags": "dcrops,dcropboost,universal-bot"
    }
}
```

16. read_comments - reads all comments done by the give author.
 1. 
  Parameters: 
    * username - a name of the user which latest comments will be read
    * commentsCount - a number of comments to read, default is 25
    * label - a prefix under which comments will be saved 
```
{
    "name": "read_comments",
    "parameters": {
        "label": "posts"
        "username": "name",
        "commentsCount": "25"
    }
}
```

16. get_commented_post - converts list of comments into a parent posts

  Parameters: 
    * label - a prefix from which comments will be read
    * parentPostLabel - a prefix under which comments will be saved 
```
{
    "name": "get_commented_post",
    "parameters": {
        "label": "posts",
        "parentPostLabel": "parent_posts"
    }
}
```

# Actions Series

There is a new way to write the similar task now. So instead of writing 5 times to stake the five various tokens you can create a one Series action definition and specify the **splitOn** to include all 5 tokens. As a result there will be 5 actinon which will have it onw token used by amount will be the same for all of them. In a case there are other parametesr they will be copied as well to a child actions.

Required parameters:
  * action - a name of the action which will be used for the child ones
  * splitParameterName - as there can be various handlers for the token this one specify what is the ": "token",
  * splitOn - put the series of parameter values in here. You can separate with comma (,) or semicolon (;)

```
{
    "name": "Series",
    "parameters": {
        "action": "Stake",
        "splitParameterName": "token",
        "splitOn": "ONEUP;CENT;PGM;ALIVE;NEOXAG;PIMP;COM",
        "amount": "*"
    }
}
```

# Time based trigger 

Every action has ability to be trigger on timely manner. For that there is a new property which contains the 6 items syntax sumilar to CRON one. The reaons there are six items is that the first one is for seconds. Seconds are important to allow bot to fullfill role of HIVE observer to react to various events. 

# Amount Calculation 

To allow slightly more advanced aproach to amont which have to be transfer there are 3 possible setting for it:
 * fixed amount like 100, it will transfer up to selected amount only
 * * -> it will transfer all availble tokesn 
 * * - 10 -> it will deduct 10 tokens from the availble amoutn and transfer the rest.

# Example configuration

``` JSON
{
  "urls": {
    "hiveNodeUrl": "https://anyx.io",
    "hiveEngineNodeUrl": "http://engine.alamut.uk:5000"
  },
  "actions": [
    {
      "name": "CustomActionName", 
      "username": "universal-bot",
      "activeKey": "<activeKey here>",
      "postingKey": "<postingKey here>",
      "trigger": "0 0 */1 * * *",
      "tasks": [
        {
          "name": "Balance" 
        },
        {
          "name": "Unstake",
          "parameters": {
            "token": "BEE",
            "amount": "*"
          }
        },
        {
          "name": "Flush"
        },
        {
          "name": "Balance"
        },
        {
          "name": "Sell",
          "parameters": {
            "token": "BEE",
            "amountToSell": "*"
          }
        },
        {
          "name": "Flush"
        }
      ]
    }
  ]
}
```

# How to support me

In a case you think that my work is useful and you want to help me, please consider supporting my Hive(https://vote.hive.uno/@assassyn) and HiveEngine witnesses (https://votify.vercel.app/alamut-he)
