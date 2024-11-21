export interface TweetForCreation
{
    mediaUrl: string | ArrayBuffer | null,
    content:string
}
export interface TweetForUpdate{
    id:number,
    mediaUrl:string|ArrayBuffer|null,
    content:string
}
export interface CreatedTweet
{
    id:number,
    mediaUrl:string|ArrayBuffer|null,
    content:string,
    userId:number,
    createdAt:Date,
    profilePicture:string |ArrayBuffer|null,
    userName:string,
    email:string,
    likesNumber:number,
    retweetsNumber:number,
    bookmarksNumber:number
}