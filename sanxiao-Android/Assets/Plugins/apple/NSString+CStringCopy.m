//
//  NSString+CStringCopy.m
//  Unity-iPhone
//
//  Created by 赵 之韵 on 13-10-21.
//
//

#import "NSString+CStringCopy.h"

@implementation NSString (CStringCopy)
-(char*) cStringCopy{
    const char *str = [self UTF8String];
    char* res = (char*)malloc(strlen(str) + 1);
    strcpy(res, str);
    return res;
}
@end
