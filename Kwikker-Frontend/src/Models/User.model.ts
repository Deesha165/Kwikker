export interface UserForCreation{
 userName:string,
 email:String,
 profilePicture:string |null,
 Bio: string | null

}

export interface CreatedUser{
    Id:number,
    UserName:string|null,
    Email:String,
    ProfilePicture:string |ArrayBuffer|null,
    CoverPicture:string|ArrayBuffer|null,
    Bio: string | null,
    CreatedAt:Date
   
}
export interface UserForUpdate
{
    Id:number,
    UserName:string|null,
    Profilepicture:string|ArrayBuffer|null,
    CoverPicture:string|ArrayBuffer|null,
    Bio: string | null,
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
accessToken:string|null,
refreshToken:string|null
}

export interface AuthenticationResponse{
    token:Token,
    userId:number
}