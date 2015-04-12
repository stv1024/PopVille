//
//  WeiboAuth.h
//  Unity-iPhone
//
//  Created by 赵 之韵 on 14-1-7.
//
//

#import <Foundation/Foundation.h>
#import "WebViewShow.h"


enum AuthType{
    Sina = 1,
    
    Tecent = 2,
    
    Renren = 3
};

enum ResponseType{
    Code = 1,
    Token = 2
};


@interface WeiboAuth : NSObject<WebPageLoaded>{
    enum AuthType _authType;
    
    enum ResponseType _responseType;
    
    NSString *_appKey;
    
    NSString *_redirectUrl;
}

-(id) initWithAuthType:(enum AuthType) authType
          responseType:(enum ResponseType) aResponseType
                appKey:(NSString *) aAppKey
           redirectUrl:(NSString *) aRedirectUrl;

-(void) startAuth;

-(void) PageLoaded:(NSString *) url;
@end