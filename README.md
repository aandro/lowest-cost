# lowest-cost

#### Technical assignment: Requesting multiple API

The concept is to request several companies' API for offers and select the best deal.

##### Conditions:

No UI expected.
No SQL required.
Must be unit-tested.

##### Process Input:

* one set of data {{source address}, {destination address}, [{carton dimensions}]}
* Multiple API using the same data with different signatures

##### Process Output:

* All API respond with the same data in different formats
* Process must query, then select the lowest offer and return it in the least amount of time
 
##### Sample APIs, each with its own url and credentials

* API1 (JSON)
                - Input {contact address, warehouse address, package dimensions:[]}
                - Output {total}
* API2 (JSON)
                - Input {consignee, consignor, cartons:[]}
                - Output {amount}
* API3 (XML)
                - Input <xml><source/><destination/><packages><package/></packages></xml>
                - Output <xml><quote/></xml>