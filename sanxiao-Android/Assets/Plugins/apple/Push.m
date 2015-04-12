//
//  Push.m
//  Unity-iPhone
//
//  Created by 赵 之韵 on 14-1-6.
//
//

#import "Push.h"
void _pushInfo(){
    NSLog(@"push info");
    [Push getPushDeviceToken];
}
@implementation Push

+ (void) getPushDeviceToken{
    [[UIApplication sharedApplication] registerForRemoteNotificationTypes:(UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound)];
}
@end
