export interface UserForCreation{
 userName:string,
 email:String,
 profilePicture:string |null,
 Bio: string | null

}

export interface CreatedUser{
    id:number,
    userName:string,
    email:String,
    profilePicture:string |null,
    Bio: string | null,
    createdAt:Date
   
}


export interface UserForRegistration{
    Email:string,
    Password:string,
    UserName:string
}

export interface UserForLogIn{
    Email:string,
    Password:string,
}

export interface Token{
accessToken:string,
refreshToken:string
}
